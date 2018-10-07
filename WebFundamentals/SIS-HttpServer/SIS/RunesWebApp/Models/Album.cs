namespace RunesWebApp.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Album : BaseEntity<string>
    {
        public Album()
        {
            this.Tracks = new HashSet<Track>();
        }

        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal Price => this.Tracks.Any() ? this.Tracks.Sum(t => t.Price) * (decimal)0.87 : 0;

        public virtual ICollection<Track> Tracks { get; set; }
    }
}
