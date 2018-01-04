﻿using System;
using System.Buffers;
using System.Buffers.Text;
using System.Data.Common;
using System.Data.SqlClient;

namespace Beeline.Writers
{
    public static class DateTimeWriter
    {
        private const byte QuotationMark = 34;
        
        public static Writer Make(int index, NameWriter name)
        {
            return (reader, buffer, pos) =>
            {
                if (reader.IsDBNull(index)) return pos;
                
                Comma.Write(buffer, ref pos);
                name.Write(buffer, ref pos);

                buffer[pos++] = QuotationMark;
                Utf8Formatter.TryFormat(reader.GetDateTime(index), buffer.Slice(pos, 32), out var n, new StandardFormat('O'));
                pos = pos + n;
                buffer[pos] = QuotationMark;
                return pos + 1;
            };
        }
    }
}