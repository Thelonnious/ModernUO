/*************************************************************************
 * ModernUO                                                              *
 * Copyright 2019-2020 - ModernUO Development Team                       *
 * Email: hi@modernuo.com                                                *
 * File: PacketUtilities.cs                                              *
 *                                                                       *
 * This program is free software: you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation, either version 3 of the License, or     *
 * (at your option) any later version.                                   *
 *                                                                       *
 * You should have received a copy of the GNU General Public License     *
 * along with this program.  If not, see <http://www.gnu.org/licenses/>. *
 *************************************************************************/

using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;

namespace Server.Network
{
    public static class PacketUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePacketLength(this CircularBufferWriter writer)
        {
            var length = writer.Position;
            writer.Seek(1, SeekOrigin.Begin);
            writer.Write((ushort)length);
            writer.Seek(length, SeekOrigin.Begin);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePacketLength(this SpanWriter writer)
        {
            var length = writer.Position;
            writer.Seek(1, SeekOrigin.Begin);
            writer.Write((ushort)length);
            writer.Seek(length, SeekOrigin.Begin);
        }

        // If LOCAL INIT is off, then stack/heap allocations have garbage data
        // Initializes the first byte (Packet ID) so it can be used as a flag.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InitializePacket(this Span<byte> buffer)
        {
#if NO_LOCAL_INIT
            buffer[0] = 0;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InitializePackets(this Span<byte> buffer, int chunkLength)
        {
#if NO_LOCAL_INIT
            var index = 0;

            while (index < buffer.Length)
            {
                buffer[index] = 0;
                index += chunkLength;
            }
#endif
        }
    }
}
