using System;
using Server.Network;
using Xunit;

namespace Server.Tests.Network
{
    public class ItemPacketTests : IClassFixture<ServerFixture>
    {
        [Fact]
        public void TestWorldItemPacket()
        {
            Serial serial = 0x1000;
            var itemId = 1;

            // Move to fixture
            TileData.ItemTable[itemId] = new ItemData(
                "Test Item Data",
                TileFlag.Generic,
                1,
                1,
                1,
                1,
                1
            );

            var item = new Item(serial)
            {
                ItemID = itemId,
                Hue = 0x1024,
                Amount = 10,
                Location = new Point3D(1000, 100, -10),
                Direction = Direction.Left
            };

            var expected = new WorldItem(item).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendWorldItem(item);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Fact]
        public void TestWorldItemSAPacket()
        {
            Serial serial = 0x1000;
            ushort itemId = 1;

            // Move to fixture
            TileData.ItemTable[itemId] = new ItemData(
                "Test Item Data",
                TileFlag.Generic,
                1,
                1,
                1,
                1,
                1
            );

            var item = new Item(serial)
            {
                ItemID = itemId,
                Hue = 0x1024,
                Amount = 10,
                Location = new Point3D(1000, 100, -10)
            };

            var expected = new WorldItemSA(item).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.ProtocolChanges = ns.ProtocolChanges | ProtocolChanges.StygianAbyss;
            ns.SendWorldItem(item);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Fact]
        public void TestWorldItemHSPacket()
        {
            Serial serial = 0x1000;
            var itemId = 1;

            // Move to fixture
            TileData.ItemTable[itemId] = new ItemData(
                "Test Item Data",
                TileFlag.Generic,
                1,
                1,
                1,
                1,
                1
            );

            var item = new Item(serial)
            {
                ItemID = itemId,
                Hue = 0x1024,
                Amount = 10,
                Location = new Point3D(1000, 100, -10)
            };

            var expected = new WorldItemHS(item).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.ProtocolChanges = ns.ProtocolChanges | ProtocolChanges.StygianAbyss | ProtocolChanges.HighSeas;
            ns.SendWorldItem(item);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }
    }
}
