using System.Xml.Serialization;

namespace PokemonFireRedClone
{
    public enum Gender {
        [XmlEnum("Male")]
        MALE,
        [XmlEnum("Female")]
        FEMALE,
        [XmlEnum("Genderless")]
        GENDERLESS }
}
