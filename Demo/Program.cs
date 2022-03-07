// See https://aka.ms/new-console-template for more information

using System.Linq.Expressions;
using System.Reflection;
using Demo;
using Wasi;
using WebAssembly.Runtime; // Acquire from https://www.nuget.org/packages/WebAssembly

using var stream = File.OpenRead(@"wasidoom.wasm");
var wasi = new WasiInstance();
var imports = new ImportDictionary
{
	{ "wasi_snapshot_preview1", "proc_exit", DelegateExtensions.AsFunctionImport<int>(wasi.proc_exit) },
	{ "wasi_snapshot_preview1", "clock_time_get", DelegateExtensions.AsFunctionImport<int, long, int, int>(wasi.clock_time_get) },
	{ "wasi_snapshot_preview1", "fd_filestat_get", DelegateExtensions.AsFunctionImport<int, int, int>(wasi.fd_filestat_get) },
	{ "wasi_snapshot_preview1", "poll_oneoff", DelegateExtensions.AsFunctionImport<int, int, int, int, int>(wasi.poll_oneoff) },
	{ "wasi_snapshot_preview1", "fd_write", DelegateExtensions.AsFunctionImport<int, int, int, int, int>(wasi.fd_write) },
	{ "wasi_snapshot_preview1", "fd_read", DelegateExtensions.AsFunctionImport<int, int, int, int, int>(wasi.fd_read) },
	{ "wasi_snapshot_preview1", "fd_close", DelegateExtensions.AsFunctionImport<int, int>(wasi.fd_close) },
	{ "wasi_snapshot_preview1", "fd_seek", DelegateExtensions.AsFunctionImport<int, long, int, int, int>(wasi.fd_seek) },
	{ "wasi_snapshot_preview1", "fd_prestat_get", DelegateExtensions.AsFunctionImport<int, int, int>(wasi.fd_prestat_get) },
	{ "wasi_snapshot_preview1", "fd_prestat_dir_name", DelegateExtensions.AsFunctionImport<int, int, int, int>(wasi.fd_prestat_dir_name) },
	{ "wasi_snapshot_preview1", "fd_fdstat_get", DelegateExtensions.AsFunctionImport<int, int, int>(wasi.fd_fdstat_get) },
	{ "wasi_snapshot_preview1", "path_open", DelegateExtensions.AsFunctionImport<int, int, int, int, int, long, long, int, int, int>(wasi.path_open) },
	{ "wasi_snapshot_preview1", "path_filestat_get", DelegateExtensions.AsFunctionImport<int, int, int, int, int, int>(wasi.path_filestat_get) },
	{ "wasi_snapshot_preview1", "path_create_directory", DelegateExtensions.AsFunctionImport<int, int, int, int>(wasi.path_create_directory) },
	{ "wasi_snapshot_preview1", "args_sizes_get", DelegateExtensions.AsFunctionImport<int, int, int>(wasi.args_sizes_get) },
	{ "wasi_snapshot_preview1", "args_get", DelegateExtensions.AsFunctionImport<int, int, int>(wasi.args_get) },
	{ "wasi_snapshot_preview1", "environ_sizes_get", DelegateExtensions.AsFunctionImport<int, int, int>(wasi.environ_sizes_get) },
	{ "wasi_snapshot_preview1", "environ_get", DelegateExtensions.AsFunctionImport<int, int, int>(wasi.environ_get) },
	{ "wasi_snapshot_preview1", "fd_fdstat_set_flags", DelegateExtensions.AsFunctionImport<int, int, int>(wasi.fd_fdstat_set_flags) },
};

wasi.set_args(args);
wasi.set_environ(new Dictionary<string, string>(){ { "HOME", "/" } });

var module = WebAssembly.Module.ReadFromBinary(stream);
/*
foreach (var import in module.Imports.Where(x => x.Module == "wasi_snapshot_preview1"))
{
	var method = typeof(wasi_unstable).GetMethod(import.Field);
	var d = method.CreateDelegate(target: null);
	imports.Add("wasi_snapshot_preview1", import.Field, new FunctionImport(d));
}
*/
var compiled = module.Compile<Foo>()(imports);
wasi.Memory = new MemoryAdaptor(compiled.Exports.memory);
wasi_unstable.set_args(args);

var env = new Dictionary<string, string>();
var foo = Environment.GetEnvironmentVariables();
foreach (var e in foo.Keys.OfType<string>().ToList())
{
	var value = (string)foo[e];
	env[e] = value;
}

wasi_unstable.set_environ(env);

compiled.Exports._start();

public abstract class Foo
{
	public abstract void _start();
	public abstract UnmanagedMemory memory { get; }
}

