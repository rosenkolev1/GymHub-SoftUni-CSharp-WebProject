using GymHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Data.Data.Configurations
{
    class ContactsChatMessageConfiguration : IEntityTypeConfiguration<ContactsChatMessage>
    {
        public void Configure(EntityTypeBuilder<ContactsChatMessage> builder)
        {
            builder
                .HasOne(x => x.Sender)
                .WithMany(x => x.ContactsChatMessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Receiver)
                .WithMany(x => x.ContactsChatMessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
