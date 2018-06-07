using System.Runtime.InteropServices;

namespace MugStore.Services.Common
{
    public static class OperatingSystemUtils
    {
        public static bool IsWindows()
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsMacOS()
            => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static bool IsLinux()
            => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static string FixOsPath(this string path)
            => IsLinux() ? path.Replace(@"\", "/") : path;
    }
}
