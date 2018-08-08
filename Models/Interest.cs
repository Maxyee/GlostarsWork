    public class Interest
    {
        public int Id { get; set; }

        public String InterestName { get; set; }

        [JsonIgnore]
        public String Extension { get; set; }

        public DateTime Date { get; set; }

        public Interest()
        {
            Date = DateTime.Now;
        }

        [NotMapped]
        public String Path
        {
            get
            {
                return ConfigurationManager.AppSettings["StorageDNS"] + "/" + BlobContainers.Pictures + "/" + Id +
                        "_interestpicture" + Extension;
            }
        }

        [NotMapped]
        [JsonIgnore]
        public String FileName
        {
            get { return Id + "_interestpicture" + Extension; }
        }


        public String GetThumb(Sizes size)
        {
            string path = "";
            path = ConfigurationManager.AppSettings["StorageDNS"];

            switch (size)
            {
                case Sizes.small:
                    path += "/" + BlobContainers.PicturesThumbs + "/" + FileName;
                    break;

                case Sizes.medium:
                    path += "/" + BlobContainers.PicturesThumbsMedium + "/" + FileName;
                    break;

                case Sizes.big:
                    path += "/" + BlobContainers.Pictures + "/" + FileName;
                    break;

                default:
                    path += "/" + BlobContainers.PicturesThumbs + "/" + FileName;
                    break;
            }
            return path;
        }


        public static class PredefinedHeights
        {
            public static int Thumb
            {
                get { return Int32.Parse(ConfigurationManager.AppSettings["ThumbSmallHeight"]); }
            }

            public static int MiniThumb
            {
                get { return Int32.Parse(ConfigurationManager.AppSettings["ThumbMiniHeight"]); }
            }

            public static int MediumThumb
            {
                get { return Int32.Parse(ConfigurationManager.AppSettings["ThumbMediumHeight"]); }
            }
        }


        public enum Sizes
        {
            medium,
            small,
            mini,
            big
        }

    }