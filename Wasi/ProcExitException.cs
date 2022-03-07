using System;

public class ProcExitException : Exception
{
	public int ReturnCode { get; private set; }
	public ProcExitException(int rc)
	{
		ReturnCode = rc;
	}
}