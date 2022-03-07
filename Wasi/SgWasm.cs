using System;
using System.Runtime.InteropServices;

public static class sg_wasm
{
	public static uint __mem_size => WasmMemory.Size;
	public static IntPtr __mem => WasmMemory.Address;

	public static IWasmMemory WasmMemory { get; set; }

	public static Span<byte> get_span(int addr, int len)
	{
		unsafe
		{
			return new Span<byte>((sg_wasm.__mem + (int) addr).ToPointer(), len);
		}
	}
	
	public interface IWasmMemory
	{
		IntPtr Address { get; }
		uint Size { get; }
	}
}