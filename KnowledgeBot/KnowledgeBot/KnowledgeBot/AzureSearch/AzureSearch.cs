using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

using KnowledgeBot.Model;
using System.Text.RegularExpressions;

namespace KnowledgeBot.AzureSearch
{
    public class AzureSearchHelper
    {
        private string _domain = "";
        private string _key = "";
        private string _index = "";
        private string _apiVersion = "";

        private string[] stopwords = new string[] { "?", ".", ",", "!", "<", ">", "a", "able", "about", "above", "abst", "accordance", "according", "accordingly", "across", "act", "actually", "added", "adj", "adopted", "affected", "affecting", "affects", "after", "afterwards", "again", "against", "ah", "all", "almost", "alone", "along", "already", "also", "although", "always", "am", "among", "amongst", "an", "and", "announce", "another", "any", "anybody", "anyhow", "anymore", "anyone", "anything", "anyway", "anyways", "anywhere", "apparently", "approximately", "are", "aren", "arent", "arise", "around", "as", "aside", "ask", "asking", "at", "auth", "available", "away", "awfully", "b", "back", "be", "became", "because", "become", "becomes", "becoming", "been", "before", "beforehand", "begin", "beginning", "beginnings", "begins", "behind", "being", "believe", "below", "beside", "besides", "between", "beyond", "biol", "both", "brief", "briefly", "but", "by", "c", "ca", "came", "can", "cannot", "can’t", "cause", "causes", "certain", "certainly", "co", "com", "come", "comes", "contain", "containing", "contains", "could", "couldnt", "d", "date", "did", "didn’t", "different", "do", "does", "doesn’t", "doing", "done", "don’t", "down", "downwards", "due", "during", "e", "each", "ed", "edu", "effect", "eg", "eight", "eighty", "either", "else", "elsewhere", "end", "ending", "enough", "especially", "et", "et-al", "etc", "even", "ever", "every", "everybody", "everyone", "everything", "everywhere", "ex", "except", "f", "far", "few", "ff", "fifth", "first", "five", "fix", "followed", "following", "follows", "for", "former", "formerly", "forth", "found", "four", "from", "further", "furthermore", "g", "gave", "get", "gets", "getting", "give", "given", "gives", "giving", "go", "goes", "gone", "got", "gotten", "h", "had", "happens", "hardly", "has", "hasn’t", "have", "haven’t", "having", "he", "hed", "hence", "her", "here", "hereafter", "hereby", "herein", "heres", "hereupon", "hers", "herself", "hes", "hi", "hid", "him", "himself", "his", "hither", "home", "how", "howbeit", "however", "hundred", "i", "id", "ie", "if", "i’ll", "im", "immediate", "immediately", "importance", "important", "in", "inc", "indeed", "index", "information", "instead", "into", "invention", "inward", "i", "is", "isn’t", "it", "itd", "it’ll", "its", "itself", "i’ve", "j", "just", "k", "keep", " keeps", "kept", "keys", "kg", "km", "know", "known", "knows", "l", "largely", "last", "lately", "later", "latter", "latterly", "least", "less", "lest", "let", "lets", "like", "liked", "likely", "line", "little", "‘ll", "look", "looking", "looks", "ltd", "m", "made", "mainly", "make", "makes", "many", "may", "maybe", "me", "mean", "means", "meantime", "meanwhile", "merely", "mg", "might", "million", "miss", "ml", "more", "moreover", "most", "mostly", "mr", "mrs", "much", "mug", "must", "my", "myself", "n", "na", "name", "namely", "nay", "nd", "near", "nearly", "necessarily", "necessary", "need", "needs", "neither", "never", "nevertheless", "new", "next", "nine", "ninety", "no", "nobody", "non", "none", "nonetheless", "noone", "nor", "normally", "nos", "not", "noted", "nothing", "now", "nowhere", "o", "obtain", "obtained", "obviously", "of", "off", "often", "oh", "ok", "okay", "old", "omitted", "on", "once", "one", "ones", "only", "onto", "or", "ord", "other", "others", "otherwise", "ought", "our", "ours", "ourselves", "out", "outside", "over", "overall", "owing", "own", "p", "page", "pages", "part", "particular", "particularly", "past", "per", "perhaps", "placed", "please", "plus", "poorly", "possible", "possibly", "potentially", "pp", "predominantly", "present", "previously", "primarily", "probably", "promptly", "proud", "provides", "put", "q", "que", "quickly", "quite", "qv", "r", "ran", "rather", "rd", "re", "readily", "really", "recent", "recently", "ref", "refs", "regarding", "regardless", "regards", "related", "relatively", "research", "respectively", "resulted", "resulting", "results", "right", "run", "s", "said", "same", "saw", "say", "saying", "says", "sec", "section", "see", "seeing", "seem", "seemed", "seeming", "seems", "seen", "self", "selves", "sent", "seven", "several", "shall", "she", "shed", "she’ll", "shes", "should", "shouldn’t", "show", "showed", "shown", "showns", "shows", "significant", "significantly", "similar", "similarly", "since", "six", "slightly", "so", "some", "somebody", "somehow", "someone", "somethan", "something", "sometime", "sometimes", "somewhat", "somewhere", "soon", "sorry", "specifically", "specified", "specify", "specifying", "state", "states", "still", "stop", "strongly", "sub", "substantially", "successfully", "such", "sufficiently", "suggest", "sup", "sure", " t", "take", "taken", "taking", "tell", "tends", "th", "than", "thank", "thanks", "thanx", "that", "that’ll", "thats", "that’ve", "the", "their", "theirs", "them", "themselves", "then", "thence", "there", "thereafter", "thereby", "thered", "therefore", "therein", "there’ll", "thereof", "therere", "theres", "thereto", "thereupon", "there’ve", "these", "they", "theyd", "they’ll", "theyre", "they’ve", "think", "this", "those", "thou", "though", "thoughh", "thousand", "throug", "through", "throughout", "thru", "thus", "til", "tip", "to", "together", "too", "took", "toward", "towards", "tried", "tries", "truly", "try", "trying", "ts", "twice", "two", "u", "un", "under", "unfortunately", "unless", "unlike", "unlikely", "until", "unto", "up", "upon", "ups", "us", "used", "useful", "usefully", "usefulness", "uses", "using", "usually", "v", "value", "various", "‘ve", "very", "via", "viz", "vol", "vols", "vs", "w", "want", "wants", "was", "wasn’t", "way", "we", "wed", "welcome", "we’ll", "went", "were", "weren’t", "we’ve", "what", "whatever", "what’ll", "whats", "when", "whence", "whenever", "where", "whereafter", "whereas", "whereby", "wherein", "wheres", "whereupon", "wherever", "whether", "which", "while", "whim", "whither", "who", "whod", "whoever", "whole", "who’ll", "whom", "whomever", "whos", "whose", "why", "widely", "willing", "wish", "with", "within", "without", "won’t", "words", "world", "would", "wouldn’t", "www", "x", "y", "yes", "yet", "you", "youd", "you’ll", "your", "youre", "yours", "yourself", "yourselves", "you’ve", "z", "zero" };


        public AzureSearchHelper(string domain, string key, string index, string apiVersion)
        {
            _domain = domain;
            _key = key;
            _index = index;
            _apiVersion = apiVersion;
        }

        public async Task<IEnumerable<SearchResult>> Search(string searchString)
        {
            try
            {
              
                // REMOVE SPECIAL CHARACTERS
                searchString = searchString
                    .Replace("?", "")
                    .Replace("!", "")
                    .Replace(".", "")
                    .Replace(",", "")
                    .Replace("<", "")
                    .Replace(">", "")
                    .Replace("#", "")
                    .Replace("&", "")
                    .Replace("|", "")
                    .Replace("(", "")
                    .Replace(")", "")
                    .Replace("{", "")
                    .Replace("}", "")
                    .Replace("[", "")
                    .Replace("]", "")
                    .Replace("^", "")
                    .Replace("\"", "")
                    .Replace("*", "")
                    .Replace(":", "")
                    .Replace("\\", "")
                    .Replace("/", "")
                    .Replace("+", "")
                    .Replace("-", "");

                // SPLIT INTO WORDS
                string[] words = searchString.Split(' ');

                #region FILTER STOP WORDS

                List<string> filteredWords = new List<string>();

                foreach (string word in words)
                {
                    string lword = word.ToLower();

                    bool isStopword = Array.Exists(
                        stopwords,
                        delegate (string s) { return s.Equals(lword); }
                    );

                    if (!isStopword)
                    {
                        filteredWords.Add(word);
                    }
                }
                #endregion


                // BUILD THE SEARCH QUERY
                string searchTitle = "(title: ";
                string searchKeywords = "(keywords: ";

                for (int i = 0; i < filteredWords.Count; i++)
                {
                    string word = filteredWords[i];

                    if (i != filteredWords.Count - 1)
                    {
                        searchTitle += word + "~  AND ";
                        searchKeywords += word + "~ ";
                    }
                    else
                    {
                        searchTitle += word + "~";
                        searchKeywords += word + "~";
                    }
                }

                searchTitle += ")";
                searchKeywords += ")";

                // BUILD THE SEARCH STRING INTO AZURE SEARCH
                string UrlRequest = "https://" + _domain + 
                                    "/indexes/" + _index + 
                                    "/docs?api-version=" + _apiVersion 
                                    + "&queryType=full&search=" + searchTitle + " " + searchKeywords;

              
                var urlResponse = await MakeRequest(UrlRequest);
                var odata = JsonConvert.DeserializeObject<ODataResponse>(urlResponse);

                return odata.Value;

            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw new Exception("Failed Contacting: " + ex.Message);
            }
        }

        private async Task<string> MakeRequest(string requestUrl)
        {
            // HTTP CLIENT
            var client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            request.Headers.Add("api-key", _key);

            HttpResponseMessage response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();

        }

        /// <summary>
        /// Replace HTML tags with Markup tags and then removes all HTML notation 
        /// allows the response to be used in the bot reply.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string StripHTML(string text)
        {
            text = replaceHyperlinks(text);
            text = replaceLineBreaks(text);
            text = replaceParagraphs(text);

            text = Regex.Replace(text, @"<[^>]+>|&rsquo;", "").Trim();
            text = Regex.Replace(text, @"&.*?;", "").Trim();

            return text;
        }



        private static string replaceParagraphs(string text)
        {
            text = text.Replace("</p>", Environment.NewLine + Environment.NewLine);
            return text;
        }

        private static string replaceLineBreaks(string text)
        {
            text = text.Replace("<br>", Environment.NewLine);
            return text;
        }

        private static string replaceHyperlinks(string text)
        {
            text = text.Replace("\"", "'");
            try
            {
                // REPLACE HYPERLINKS
                string pat = @"<a(.)+<\/a>";
                Regex r = new Regex(pat, RegexOptions.IgnoreCase);
                Match m = r.Match(text);
                if (m.Success)
                {

                    string value = m.Value;
                    string replacement = replacehyperlink(value);

                    text = text.Replace(value, replacement);

                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;

            }

            return text;
        }

        private static string replacehyperlink(string hyperlinktext)
        {
            string newHrefText = "";
            string label = "";
            string linkTextminusHref = "";


            string pat = @"href='[^']+'";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match m = r.Match(hyperlinktext);
            if (m.Success)
            {
                Group g = m.Groups[0];
                CaptureCollection cc = g.Captures;
                Capture c = cc[0];
                string linktext = c.Value;

                linkTextminusHref = linktext.Replace("href='", "").Replace("'", "");
            }

            pat = @"(?<=<a href='[^']+'>)(.*)(?=<\/a\>)";
            r = new Regex(pat, RegexOptions.IgnoreCase);
            m = r.Match(hyperlinktext);
            if (m.Success)
            {
                Group g = m.Groups[0];
                CaptureCollection cc = g.Captures;
                Capture c = cc[0];
                label = c.Value;
            }

            newHrefText = "[" + label + "](" + linkTextminusHref + ")";
            return newHrefText;
        }
    }
}