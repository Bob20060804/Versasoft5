using System;

namespace BR.AN.PviServices
{
	public class ARMRegisters
	{
		[CLSCompliant(false)]
		public uint GeneralPurposeRegister00
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint GeneralPurposeRegister01
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint GeneralPurposeRegister02
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint GeneralPurposeRegister03
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint GeneralPurposeRegister04
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint GeneralPurposeRegister05
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint GeneralPurposeRegister06
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint GeneralPurposeRegister07
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint GeneralPurposeRegister08
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint GeneralPurposeRegister09
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint GeneralPurposeRegister10
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint FramePointer
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint GeneralPurposeRegister12
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint StackPointer
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint LinkRegister
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint ProgramCounter
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint CurrentProgramStatusRegister
		{
			get;
			internal set;
		}

		[CLSCompliant(false)]
		public uint TranslationTableBaseControlRegister
		{
			get;
			internal set;
		}

		internal ARMRegisters(ARMExceptionTaskInfo taskInfo)
		{
			GeneralPurposeRegister00 = taskInfo.ulR0;
			GeneralPurposeRegister01 = taskInfo.ulR1;
			GeneralPurposeRegister02 = taskInfo.ulR2;
			GeneralPurposeRegister03 = taskInfo.ulR3;
			GeneralPurposeRegister04 = taskInfo.ulR4;
			GeneralPurposeRegister05 = taskInfo.ulR5;
			GeneralPurposeRegister06 = taskInfo.ulR6;
			GeneralPurposeRegister07 = taskInfo.ulR7;
			GeneralPurposeRegister08 = taskInfo.ulR8;
			GeneralPurposeRegister09 = taskInfo.ulR9;
			GeneralPurposeRegister10 = taskInfo.ulR10;
			FramePointer = taskInfo.ulFp;
			GeneralPurposeRegister12 = taskInfo.ulR12;
			StackPointer = taskInfo.ulSp;
			LinkRegister = taskInfo.ulLr;
			ProgramCounter = taskInfo.ulPc;
			CurrentProgramStatusRegister = taskInfo.ulCpsr;
			TranslationTableBaseControlRegister = taskInfo.ulTtbase;
		}

		internal ARMRegisters()
		{
			GeneralPurposeRegister00 = 0u;
			GeneralPurposeRegister01 = 0u;
			GeneralPurposeRegister02 = 0u;
			GeneralPurposeRegister03 = 0u;
			GeneralPurposeRegister04 = 0u;
			GeneralPurposeRegister05 = 0u;
			GeneralPurposeRegister06 = 0u;
			GeneralPurposeRegister07 = 0u;
			GeneralPurposeRegister08 = 0u;
			GeneralPurposeRegister09 = 0u;
			GeneralPurposeRegister10 = 0u;
			FramePointer = 0u;
			GeneralPurposeRegister12 = 0u;
			StackPointer = 0u;
			LinkRegister = 0u;
			ProgramCounter = 0u;
			CurrentProgramStatusRegister = 0u;
			TranslationTableBaseControlRegister = 0u;
		}
	}
}
