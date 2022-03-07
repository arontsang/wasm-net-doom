using WebAssembly.Runtime;

namespace Demo;

public class MemoryAdaptor
	: sg_wasm.IWasmMemory
{
	private UnmanagedMemory _memory;

	public MemoryAdaptor(UnmanagedMemory memory)
	{
		_memory = memory;
	}

	public uint Size => _memory.Size;
	public IntPtr Address => _memory.Start;
}