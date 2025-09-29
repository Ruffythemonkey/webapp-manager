namespace FoxyWebappManager.Models
{
    public class UserChrome
    {
        public readonly List<string> UserChromeSettings = new List<string>()
        {
            ":root[taskbartab] #urlbar-container {\r\n    display: none !important;\r\n}",
            ":root[taskbartab] hbox.titlebar-spacer {\r\n    display: none !important;\r\n}",
            ":root[taskbartab] toolbar {\r\n    .chromeclass-toolbar-additional\r\n\r\n{\r\n    display: none;\r\n}\r\n\r\n#back-button,\r\n#forward-button,\r\n#stop-reload-button,\r\n#customizableui-special-spring1,\r\n#customizableui-special-spring2,\r\n#downloads-button,\r\n#unified-extensions-button,\r\n.unified-extensions-item,\r\n#save-page-button,\r\n#print-button,\r\n#find-button,\r\n#zoom-controls,\r\n#edit-controls,\r\n#characterencoding-button,\r\n#email-link-button,\r\n#screenshot-button,\r\n#nav-bar-overflow-button,\r\n#taskbar-tabs-audio:is([soundplaying], [muted]),\r\n#fullscreen-button {\r\n    display: table;\r\n}\r\n}"
        };
    }
}
