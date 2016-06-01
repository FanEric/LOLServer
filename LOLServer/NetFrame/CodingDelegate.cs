using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    public delegate byte[] LengthEncode(byte[] value);
    public delegate byte[] LengthDecode(ref List<byte> value);

    public delegate byte[] Encode(Object value);
    public delegate object Decode(byte[] value);
}
