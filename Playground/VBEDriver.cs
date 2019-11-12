using System;
using Uskr.Runtime;

namespace Playground
{
    public class VBEDriver
    {
        private enum RegisterIndex : ushort
        {
            DisplayID = 0x00,
            DisplayXResolution = 1,
            DisplayYResolution = 2,
            DisplayBPP = 3,
            DisplayEnable = 4,
            DisplayBankMode = 5,
            DisplayVirtualWidth = 6,
            DisplayVirtualHeight = 7,
            DisplayXOffset = 8,
            DisplayYOffse = 9
        };

        [Flags]
        private enum EnableValues
        {
            Disabled = 0x00,
            Enabled,
            UseLinearFrameBuffer = 0x40,
            NoClearMemory = 0x80,
        };


 

        private void Write(RegisterIndex index, ushort value)
        {
            KIO.outpw(0x01CE, (ushort) index);
            KIO.outpw(0x01CF, value);
        }

        private void DisableDisplay()
        {
            Write(RegisterIndex.DisplayEnable, (ushort) EnableValues.Disabled);
        }

        private void SetXResolution(ushort xres)
        {
            Write(RegisterIndex.DisplayXResolution, xres);
        }

        private void SetYResolution(ushort yres)
        {
            Write(RegisterIndex.DisplayYResolution, yres);
        }

        private void SetDisplayBPP(ushort bpp)
        {
            Write(RegisterIndex.DisplayBPP, bpp);
        }

        private void EnableDisplay(EnableValues EnableFlags)
        {
            
            Write(RegisterIndex.DisplayEnable, (ushort) EnableFlags);
        }

        public void VBESet(ushort xres, ushort yres, ushort bpp)
        {
            DisableDisplay();
            SetXResolution(xres);
            SetYResolution(yres);
            SetDisplayBPP(bpp);
            /*
             * Re-enable the Display with LinearFrameBuffer and without clearing video memory of previous value 
             * (this permits to change Mode without losing the previous datas)
             */
            EnableDisplay(EnableValues.Enabled | EnableValues.UseLinearFrameBuffer | EnableValues.NoClearMemory);
        }
    }
}