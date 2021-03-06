namespace Server.Items
{
    public class SerratedWarCleaver : WarCleaver
    {
        [Constructible]
        public SerratedWarCleaver() => Attributes.WeaponDamage = 7;

        public SerratedWarCleaver(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1073527; // serrated war cleaver

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadEncodedInt();
        }
    }
}
