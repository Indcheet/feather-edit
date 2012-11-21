using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace CursesSharp.LearnTest {

    public class ConsoleUtil {

        public static void Start() {
            AllocConsole();
        }

        public static void End() {
            FreeConsole();
        }

        public static string ReadConsoleAsString() {
            var stdOut = GetConsole();
            var size = GetConsoleSize(stdOut);
            return DumpConsoleContent(stdOut, NrCharactersFrom(size));
        }

        static IntPtr GetConsole() {
            const int stdOutputHandle = -11;
            return GetStdHandle(stdOutputHandle);
        }

        static Coord GetConsoleSize(IntPtr stdOut) {
            var bufferInfo = ConsoleScreenBufferInfoEx.Create();
            if (!GetConsoleScreenBufferInfoEx(stdOut, ref bufferInfo))
                throw new Win32Exception();
            return bufferInfo.dwMaximumWindowSize;
        }

        static string DumpConsoleContent(IntPtr stdOut, int length) {
            uint charsRead;
            var output = new StringBuilder(length);
            ReadConsoleOutputCharacter(stdOut, output, (uint) length, new Coord(), out charsRead);
            return output.ToString();
        }

        static int NrCharactersFrom(Coord size) {
            return size.x * size.y;
        }

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool AllocConsole();

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool FreeConsole();

        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool ReadConsoleOutputCharacter(IntPtr hConsoleOutput,
            [Out]StringBuilder lpCharacter, uint nLength, Coord dwReadCoord,
            out uint lpNumberOfCharsRead);

        [DllImport("kernel32", SetLastError = true)]
        static extern bool GetConsoleScreenBufferInfoEx(
            IntPtr hConsoleOutput,
            ref ConsoleScreenBufferInfoEx consoleScreenBufferInfo);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ConsoleScreenBufferInfoEx {
        public uint cbSize;
        public Coord dwSize;
        public Coord dwCursorPosition;
        public short wAttributes;
        public SmallRect srWindow;
        public Coord dwMaximumWindowSize;

        public ushort wPopupAttributes;
        public bool bFullscreenSupported;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public uint[] colorTable;

        public static ConsoleScreenBufferInfoEx Create() {
            return new ConsoleScreenBufferInfoEx { cbSize = 96 };
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Coord {
        public short x;
        public short y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SmallRect {
        public short left;
        public short top;
        public short right;
        public short bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ColorRef {

        public uint colorDWORD;

        public ColorRef(Color color) {
            colorDWORD = color.R + ((uint) color.G << 8) + ((uint) color.B << 16);
        }

        public Color GetColor() {
            return Color.FromArgb(
                (int) (0x000000FFU & colorDWORD),
                (int) (0x0000FF00U & colorDWORD) >> 8,
                (int) (0x00FF0000U & colorDWORD) >> 16);
        }

        public void SetColor(Color color) {
            colorDWORD = color.R + (((uint) color.G) << 8) + (((uint) color.B) << 16);
        }

    }
}