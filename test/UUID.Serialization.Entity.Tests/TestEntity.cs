using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UUIDSerializationEntityTests
{
    public class TestEntity
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "BLOB")]
        public UUID ByteUUID { get; set; }

        [Column(TypeName = "TEXT")]
        public UUID StringUUID { get; set; }

        [Column(TypeName = "TEXT")]
        public UUID Base64UUID { get; set; }
    }
}