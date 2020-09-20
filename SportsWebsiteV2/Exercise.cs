using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebsiteV2
{
    public class Exercise
    {
        public string Link;
        public string Name;
        public string Reps;
        public string Sets;

        public Exercise(string name, bool IsCompund)
        {
            this.Name = name;
            this.Link = YTLinkGenerator(name);
            RepsSetsGenerator(IsCompund);
        }

        private string YTLinkGenerator(string name)
        {//bulid a yt link to the search of this exc name by adding the bane if only one word but if more it put + sign between the words
            string link = " https://www.youtube.com/results?search_query=",
                word = "";
            bool first = true;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == ' ')
                {
                    if (!first)
                    {
                        link += "+" + word;
                    }
                    else
                    {
                        link += word;
                        first = false;
                    }
                    word = "";
                }
                else
                {
                    word += name[i];
                }
            }
            link += word;
            return link;
        }

        private void RepsSetsGenerator(bool IsCompund)
        {
            if (IsCompund)
            {
                this.Reps = "6";
                this.Sets = "4";
            }
            else
            {
                this.Reps = "12";
                this.Sets = "3";
            }
        }
    }
}