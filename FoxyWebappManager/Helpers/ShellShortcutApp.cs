using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.InteropServices.ComTypes;

namespace FoxyWebappManager.Helpers
{
    public static class ShellShortcutApp
    {

       public static void CreateShortcutWithAppId(string shortcutPath, string targetExe, string arguments, string iconLocation, string appId)
        {
            // COM: IShellLink erzeugen
            IShellLinkW link = (IShellLinkW)new CShellLink();
            link.SetPath(targetExe);
            link.SetArguments(arguments);
            link.SetWorkingDirectory(Path.GetDirectoryName(targetExe)!);
            link.SetIconLocation(iconLocation, 0);
            link.SetDescription("Meine WebApp - " + Path.GetFileNameWithoutExtension(shortcutPath));

            // PropertyStore (AppUserModelID) setzen
            IPropertyStore propStore = (IPropertyStore)link;
            PropertyKey key = PropertyKey.AppUserModel_ID;

            PropVariant pv = new PropVariant(appId);
            try
            {
                propStore.SetValue(ref key, ref pv);
                propStore.Commit();
            }
            finally
            {
                pv.Dispose();
            }

            // Shortcut speichern
            IPersistFile persist = (IPersistFile)link;
            persist.Save(shortcutPath, true);
        }

        static string ReadAppIdFromShortcut(string shortcutPath)
        {
            IShellLinkW link = (IShellLinkW)new CShellLink();
            ((IPersistFile)link).Load(shortcutPath, 0);

            IPropertyStore propStore = (IPropertyStore)link;
            PropertyKey key = PropertyKey.AppUserModel_ID;
            PropVariant pv;
            propStore.GetValue(ref key, out pv);

            string result = null!;
            try
            {
                if (pv.vt == (ushort)VarEnum.VT_LPWSTR && pv.pointerValue != IntPtr.Zero)
                    result = Marshal.PtrToStringUni(pv.pointerValue)!;
            }
            finally
            {
                // PropVariant freigeben
                PropVariantClear(ref pv);
            }
            return result!;
        }

        #region COM-Interop und Hilfsstrukturen

        [ComImport, Guid("00021401-0000-0000-C000-000000000046")]
        class CShellLink { }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214F9-0000-0000-C000-000000000046")]
        interface IShellLinkW
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cch, ref IntPtr pfd, uint fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);
            void Resolve(IntPtr hwnd, uint fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")]
        interface IPropertyStore
        {
            void GetCount(out uint cProps);
            void GetAt([In] uint iProp, out PropertyKey pkey);
            void GetValue([In] ref PropertyKey key, out PropVariant pv);
            void SetValue([In] ref PropertyKey key, [In] ref PropVariant pv);
            void Commit();
        }

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

        [DllImport("ole32.dll")]
        static extern int PropVariantClear(ref PropVariant pvar);

        #endregion
    }
}
