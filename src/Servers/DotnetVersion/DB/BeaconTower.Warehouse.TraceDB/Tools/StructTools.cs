using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.TraceDB.Tools
{
    internal static class StructTools
    {
        public static ref T ToStruct<T>(Span<byte> data) where T : struct
        {
            if (Unsafe.SizeOf<T>() > (uint)data.Length)
            {
                throw new ArgumentException($"ArgumentOutOfRangeException because", nameof(data));
            }

            ref var start = ref MemoryMarshal.GetReference(data);
            IntPtr addr;
            unsafe { addr = (IntPtr)Unsafe.AsPointer<byte>(ref start); };
            object tmp = Marshal.PtrToStructure<T>(addr);
            return ref Unsafe.Unbox<T>(tmp);
        }
    }
}
