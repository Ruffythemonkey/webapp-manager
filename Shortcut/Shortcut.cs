using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Shortcut
{
    public static class Shortcut
    {
        // GUIDs
        static Guid CLSID_ShellLink = new Guid("00021401-0000-0000-C000-000000000046");
        static Guid IID_IShellLinkW = new Guid("000214F9-0000-0000-C000-000000000046");
        static Guid IID_IPersistFile = new Guid("0000010b-0000-0000-C000-000000000046");
        static Guid IID_IPropertyStore = new Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99");


        // CoInitializeEx constants
        const uint COINIT_APARTMENTTHREADED = 0x2;
        const int S_OK = 0;

        // P/Invoke
        [DllImport("ole32.dll")]
        static extern int CoInitializeEx(IntPtr pvReserved, uint dwCoInit);

        [DllImport("ole32.dll")]
        static extern void CoUninitialize();

        [DllImport("ole32.dll", ExactSpelling = true, PreserveSig = true)]
        static extern int CoCreateInstance([In] ref Guid rclsid, IntPtr pUnkOuter, uint dwClsContext, [In] ref Guid riid, out IntPtr ppv);

        const uint CLSCTX_INPROC_SERVER = 1;

        [DllImport("ole32.dll", PreserveSig = true)]
        static extern int PropVariantClear(ref PropVariant pvar);

        // PropertyKey and PropVariant - compatible with original
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct PropertyKey
        {
            public Guid fmtid;
            public uint pid;
            public static PropertyKey AppUserModel_ID => new PropertyKey
            {
                fmtid = new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"),
                pid = 5
            };
        }

        [StructLayout(LayoutKind.Explicit)]
        struct PropVariant : IDisposable
        {
            [FieldOffset(0)] public ushort vt;
            [FieldOffset(2)] public ushort wReserved1;
            [FieldOffset(4)] public ushort wReserved2;
            [FieldOffset(6)] public ushort wReserved3;
            [FieldOffset(8)] public IntPtr pointerValue;

            public PropVariant(string val)
            {
                vt = (ushort)VarEnum.VT_LPWSTR;
                wReserved1 = wReserved2 = wReserved3 = 0;
                pointerValue = Marshal.StringToCoTaskMemUni(val);
            }

            public void Dispose()
            {
                PropVariantClear(ref this);
                GC.SuppressFinalize(this);
            }
        }

        enum VarEnum : ushort
        {
            VT_EMPTY = 0,
            VT_LPWSTR = 31
        }

        // Helpers for COM VTable calling
        static IntPtr ReadVTableFuncPtr(IntPtr comObject, int vtableIndex)
        {
            // comObject is a pointer to an interface (pointer to vtable) - read vtable pointer then the function pointer
            IntPtr vtable = Marshal.ReadIntPtr(comObject); // points to vtable start
            return Marshal.ReadIntPtr(vtable, vtableIndex * IntPtr.Size);
        }

        // QueryInterface / Release helpers
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate int QueryInterfaceDelegate(IntPtr thisPtr, ref Guid riid, out IntPtr ppvObject);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate uint AddRefDelegate(IntPtr thisPtr);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate uint ReleaseDelegate(IntPtr thisPtr);

        static IntPtr QueryInterfaceRaw(IntPtr ifacePtr, ref Guid iid)
        {
            var qiPtr = ReadVTableFuncPtr(ifacePtr, 0); // QueryInterface is at index 0 of vtable (but vtable pointer sits at comObject[0])
            var qi = Marshal.GetDelegateForFunctionPointer<QueryInterfaceDelegate>(qiPtr);
            qi(ifacePtr, ref iid, out IntPtr ppv);
            return ppv;
        }

        static void ReleaseRaw(IntPtr ifacePtr)
        {
            if (ifacePtr == IntPtr.Zero) return;
            var relPtr = ReadVTableFuncPtr(ifacePtr, 2); // Release is vtable index 2
            var rel = Marshal.GetDelegateForFunctionPointer<ReleaseDelegate>(relPtr);
            rel(ifacePtr);
        }

        // IShellLinkW methods (vtable indices relative to interface):
        // After IUnknown (QueryInterface(0), AddRef(1), Release(2)), methods start:
        // 0 GetPath, 1 GetIDList, 2 SetIDList, 3 GetDescription, 4 SetDescription, 5 GetWorkingDirectory,
        // 6 SetWorkingDirectory, 7 GetArguments, 8 SetArguments, 9 GetHotkey, 10 SetHotkey, 11 GetShowCmd,
        // 12 SetShowCmd, 13 GetIconLocation, 14 SetIconLocation, 15 SetRelativePath, 16 Resolve, 17 SetPath
        // So SetPath is vtable index 3 + 17 = 20 (absolute).
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        delegate int SetPathDelegate(IntPtr thisPtr, [MarshalAs(UnmanagedType.LPWStr)] string pszFile);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        delegate int SetArgumentsDelegate(IntPtr thisPtr, [MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        delegate int SetWorkingDirectoryDelegate(IntPtr thisPtr, [MarshalAs(UnmanagedType.LPWStr)] string pszDir);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        delegate int SetIconLocationDelegate(IntPtr thisPtr, [MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        delegate int SetDescriptionDelegate(IntPtr thisPtr, [MarshalAs(UnmanagedType.LPWStr)] string pszName);

        // IPersistFile Save/Load
        // IPersistFile methods after IUnknown:
        // 0 GetClassID, 1 IsDirty, 2 Load, 3 Save, 4 SaveCompleted, 5 GetCurFile
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        delegate int LoadDelegate(IntPtr thisPtr, [MarshalAs(UnmanagedType.LPWStr)] string pszFileName, uint mode);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        delegate int SaveDelegate(IntPtr thisPtr, [MarshalAs(UnmanagedType.LPWStr)] string pszFileName, bool remember);

        // IPropertyStore delegates - methods after IUnknown:
        // 0 GetCount, 1 GetAt, 2 GetValue, 3 SetValue, 4 Commit
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate int SetValueDelegate(IntPtr thisPtr, ref PropertyKey key, ref PropVariant pv);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate int CommitDelegate(IntPtr thisPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate int GetValueDelegate(IntPtr thisPtr, ref PropertyKey key, out PropVariant pv);

        // Main public methods
        public static void CreateShortcutWithAppId(string shortcutPath, string targetExe, string arguments, string iconLocation, string appId)
        {
            if (string.IsNullOrEmpty(shortcutPath)) throw new ArgumentNullException(nameof(shortcutPath));
            if (string.IsNullOrEmpty(targetExe)) throw new ArgumentNullException(nameof(targetExe));

            int hr = CoInitializeEx(IntPtr.Zero, COINIT_APARTMENTTHREADED);
            if (hr != S_OK && hr != 0x00000001 /*S_FALSE*/)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            IntPtr shellLinkPtr = IntPtr.Zero;
            IntPtr persistFilePtr = IntPtr.Zero;
            IntPtr propStorePtr = IntPtr.Zero;

            try
            {
                // Create ShellLink COM object
                hr = CoCreateInstance(ref CLSID_ShellLink, IntPtr.Zero, CLSCTX_INPROC_SERVER, ref IID_IShellLinkW, out shellLinkPtr);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                // --- SetPath ---
                IntPtr setPathPtr = ReadVTableFuncPtr(shellLinkPtr, 3 + 17); // absolute vtable index for SetPath
                var setPath = Marshal.GetDelegateForFunctionPointer<SetPathDelegate>(setPathPtr);
                hr = setPath(shellLinkPtr, targetExe);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                // SetArguments
                IntPtr setArgsPtr = ReadVTableFuncPtr(shellLinkPtr, 3 + 8);
                var setArgs = Marshal.GetDelegateForFunctionPointer<SetArgumentsDelegate>(setArgsPtr);
                hr = setArgs(shellLinkPtr, arguments ?? string.Empty);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                // SetWorkingDirectory
                string workingDir = Path.GetDirectoryName(targetExe) ?? string.Empty;
                IntPtr setWorkingDirPtr = ReadVTableFuncPtr(shellLinkPtr, 3 + 6);
                var setWorkingDir = Marshal.GetDelegateForFunctionPointer<SetWorkingDirectoryDelegate>(setWorkingDirPtr);
                hr = setWorkingDir(shellLinkPtr, workingDir);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                // SetIconLocation
                IntPtr setIconPtr = ReadVTableFuncPtr(shellLinkPtr, 3 + 14);
                var setIcon = Marshal.GetDelegateForFunctionPointer<SetIconLocationDelegate>(setIconPtr);
                hr = setIcon(shellLinkPtr, iconLocation ?? string.Empty, 0);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                // SetDescription
                string desc = $"{Path.GetFileNameWithoutExtension(shortcutPath)} Web-App";
                IntPtr setDescPtr = ReadVTableFuncPtr(shellLinkPtr, 3 + 4);
                var setDesc = Marshal.GetDelegateForFunctionPointer<SetDescriptionDelegate>(setDescPtr);
                hr = setDesc(shellLinkPtr, desc);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                // Query IPersistFile
                persistFilePtr = QueryInterfaceRaw(shellLinkPtr, ref IID_IPersistFile);
                if (persistFilePtr == IntPtr.Zero) throw new InvalidOperationException("IPersistFile not supported");

                // Query IPropertyStore
                propStorePtr = QueryInterfaceRaw(shellLinkPtr, ref IID_IPropertyStore);
                if (propStorePtr == IntPtr.Zero) throw new InvalidOperationException("IPropertyStore not supported");

                // Set AppUserModelID using IPropertyStore.SetValue + Commit
                PropertyKey key = PropertyKey.AppUserModel_ID;
                PropVariant pv = new PropVariant(appId ?? string.Empty);
                try
                {
                    IntPtr setValuePtr = ReadVTableFuncPtr(propStorePtr, 3 + 3); // IPropertyStore.SetValue is index 3 after IUnknown
                    var setValue = Marshal.GetDelegateForFunctionPointer<SetValueDelegate>(setValuePtr);
                    hr = setValue(propStorePtr, ref key, ref pv);
                    if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                    IntPtr commitPtr = ReadVTableFuncPtr(propStorePtr, 3 + 4);
                    var commit = Marshal.GetDelegateForFunctionPointer<CommitDelegate>(commitPtr);
                    hr = commit(propStorePtr);
                    if (hr != 0) Marshal.ThrowExceptionForHR(hr);
                }
                finally
                {
                    // PropVariantClear handled by Dispose
                    pv.Dispose();
                }

                // Save using IPersistFile.Save
                IntPtr savePtr = ReadVTableFuncPtr(persistFilePtr, 3 + 3); // Save is index 3 after IUnknown in IPersistFile
                var save = Marshal.GetDelegateForFunctionPointer<SaveDelegate>(savePtr);

                // Ensure dir exists
                string dir = Path.GetDirectoryName(shortcutPath) ?? Environment.CurrentDirectory;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                hr = save(persistFilePtr, shortcutPath, true);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);
            }
            finally
            {
                if (propStorePtr != IntPtr.Zero) ReleaseRaw(propStorePtr);
                if (persistFilePtr != IntPtr.Zero) ReleaseRaw(persistFilePtr);
                if (shellLinkPtr != IntPtr.Zero) ReleaseRaw(shellLinkPtr);
                CoUninitialize();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Manual cleanup using PropVariantClear/ReleaseRaw")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Windows-only API")]
        public static string ReadAppIdFromShortcut(string shortcutPath)
        {
            if (string.IsNullOrEmpty(shortcutPath)) throw new ArgumentNullException(nameof(shortcutPath));
            if (!File.Exists(shortcutPath)) throw new FileNotFoundException("Shortcut not found", shortcutPath);

            int hr = CoInitializeEx(IntPtr.Zero, COINIT_APARTMENTTHREADED);
            if (hr != S_OK && hr != 0x00000001 /*S_FALSE*/)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            IntPtr shellLinkPtr = IntPtr.Zero;
            IntPtr persistFilePtr = IntPtr.Zero;
            IntPtr propStorePtr = IntPtr.Zero;

            try
            {
                hr = CoCreateInstance(ref CLSID_ShellLink, IntPtr.Zero, CLSCTX_INPROC_SERVER, ref IID_IShellLinkW, out shellLinkPtr);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                // Query IPersistFile and Load
                persistFilePtr = QueryInterfaceRaw(shellLinkPtr, ref IID_IPersistFile);
                if (persistFilePtr == IntPtr.Zero) throw new InvalidOperationException("IPersistFile not supported");

                IntPtr loadPtr = ReadVTableFuncPtr(persistFilePtr, 3 + 2); // Load is index 2 after IUnknown
                var load = Marshal.GetDelegateForFunctionPointer<LoadDelegate>(loadPtr);
                hr = load(persistFilePtr, shortcutPath, 0);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                // Query IPropertyStore
                propStorePtr = QueryInterfaceRaw(shellLinkPtr, ref IID_IPropertyStore);
                if (propStorePtr == IntPtr.Zero) throw new InvalidOperationException("IPropertyStore not supported");

                // Get value
                PropertyKey key = PropertyKey.AppUserModel_ID;
                PropVariant pv;
                IntPtr getValuePtr = ReadVTableFuncPtr(propStorePtr, 3 + 2); // GetValue is index 2 after IUnknown
                var getValue = Marshal.GetDelegateForFunctionPointer<GetValueDelegate>(getValuePtr);
                hr = getValue(propStorePtr, ref key, out pv);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                try
                {
                    if (pv.vt == (ushort)VarEnum.VT_LPWSTR && pv.pointerValue != IntPtr.Zero)
                    {
                        string s = Marshal.PtrToStringUni(pv.pointerValue) ?? string.Empty;
                        return s;
                    }
                    return string.Empty;
                }
                finally
                {
                    // Free PropVariant memory
                    PropVariantClear(ref pv);
                }
            }
            finally
            {
                if (propStorePtr != IntPtr.Zero) ReleaseRaw(propStorePtr);
                if (persistFilePtr != IntPtr.Zero) ReleaseRaw(persistFilePtr);
                if (shellLinkPtr != IntPtr.Zero) ReleaseRaw(shellLinkPtr);
                CoUninitialize();
            }
        }
    }
}
