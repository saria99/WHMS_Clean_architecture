using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodwitWHMS.Infrastructures.BarCode;
public interface IBarcodeGenerator
{
    byte[] GenerateBarcode(string content, int width, int height);
}
