using Lab02Rudnyk.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab02Rudnyk.Repositories
{
    internal class PersonManager
    {
        private static readonly string BaseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "UsersStorage");

        public PersonManager()
        {
            if (!Directory.Exists(BaseFolder))
            {
                Directory.CreateDirectory(BaseFolder);
            }
            if (!Directory.EnumerateFiles(BaseFolder).Any())
            {
                Task.Run(async () => await CreatePersons()).Wait();
            }
        }
        public async Task AddOrUpdatePerson(Person person)
        {

            var stringPerson = JsonSerializer.Serialize(person);

            string filePath = Path.Combine(BaseFolder, person.EmailAddress + ".txt");

            using (StreamWriter sw = new StreamWriter(filePath,false))
            {
                await sw.WriteAsync(stringPerson);
            }
        }

        public async Task<Person> GetPerson(string email)
        {
            string personStr = null;
            string filePath = Path.Combine(BaseFolder, email);

            if (!File.Exists(filePath))
            {
                return null;
            }

            using(StreamReader sr = new StreamReader(filePath))
            {
                personStr = await sr.ReadToEndAsync();
            }
            return JsonSerializer.Deserialize<Person>(personStr);
        }

        public List<Person> GetAllPersons()
        {
            var res = new List<Person>();

            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                string personStr = null;

                using(StreamReader reader = new StreamReader(file))
                {
                    personStr = reader.ReadToEnd();
                }

                res.Add(JsonSerializer.Deserialize<Person>(personStr));
            }
            return res;
        }

        public async void DeletePerson(string email)
        {
            string filePath = Path.Combine(BaseFolder, email + ".txt");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private async Task CreatePersons()
        {
            var userData = @"Bethany,Yarrington,byarrington1@cafepress.com,7/22/2009
                Ibrahim,Lathleiffure,ilathleiffure0@list-manage.com,2/24/2012
                Benn,Rens,brens2@nasa.gov,10/31/1986
                Emelia,Boulter,eboulter3@google.it,9/15/2014
                Avigdor,Ventham,aventham4@mlb.com,5/30/1997
                Merrielle,Barthorpe,mbarthorpe5@cmu.edu,1/3/1987
                Amby,Hansley,ahansley6@dyndns.org,3/1/1994
                Iormina,Wilkenson,iwilkenson7@unc.edu,12/1/2006
                Valry,Duell,vduell8@dell.com,12/9/1993
                Renard,Woollhead,rwoollhead9@samsung.com,6/2/1991
                Kizzie,Clacson,kclacsona@histats.com,1/5/2014
                Druci,Scrymgeour,dscrymgeourb@army.mil,9/10/1986
                Kylila,Stichel,kstichelc@about.com,5/25/2013
                Odella,Bendtsen,obendtsend@exblog.jp,7/24/1986
                Townie,Birkwood,tbirkwoode@technorati.com,10/9/1999
                Sibley,Goldup,sgoldupf@acquirethisname.com,1/25/2012
                Nannie,Beyne,nbeyneg@google.ca,2/5/2006
                Obie,Gregoire,ogregoireh@deliciousdays.com,2/16/2013
                Jerald,Chalfont,jchalfonti@washingtonpost.com,5/4/2007
                Dell,Bratt,dbrattj@amazonaws.com,8/14/1990
                Cliff,De Bruijn,cdebruijnk@sbwire.com,3/4/1999
                Heidie,Matschke,hmatschkel@usatoday.com,1/5/2014
                Siana,Kolak,skolakm@so-net.ne.jp,4/1/1994
                Jolie,Barg,jbargn@51.la,9/24/2013
                Briana,Sandbatch,bsandbatcho@usatoday.com,1/20/2008
                Coral,Kiehnlt,ckiehnltp@princeton.edu,11/2/2010
                Lennard,Beeson,lbeesonq@answers.com,2/13/1997
                Bobbi,Myatt,bmyattr@quantcast.com,12/1/1993
                Ozzy,Pitkethly,opitkethlys@istockphoto.com,3/15/2010
                Catherina,Stoyell,cstoyellt@google.it,12/30/1992
                Opal,Raffon,oraffonu@cbsnews.com,3/21/2006
                Shelby,Jobson,sjobsonv@cnet.com,2/23/2009
                Sarajane,Purviss,spurvissw@blog.com,12/2/1992
                Iseabal,Newton,inewtonx@gnu.org,5/4/1992
                Virginie,Poate,vpoatey@ocn.ne.jp,8/18/2010
                Tillie,Reilinger,treilingerz@reuters.com,11/23/2000
                Gayle,Acton,gacton10@nba.com,6/4/1995
                Thedrick,Marguerite,tmarguerite11@dropbox.com,8/9/1995
                Damian,Desantis,ddesantis12@newsvine.com,4/28/2002
                Genny,Whiting,gwhiting13@sina.com.cn,4/6/2002
                Ellen,Yanyshev,eyanyshev14@behance.net,2/9/1990
                Gabriel,Southard,gsouthard15@ebay.co.uk,10/16/2014
                Marten,Pering,mpering16@usgs.gov,7/10/2011
                Jacqui,Apted,japted17@php.net,11/23/2003
                Tybi,McIlroy,tmcilroy18@foxnews.com,8/9/1994
                Francisca,Varne,fvarne19@ibm.com,11/4/1987
                Mirabelle,Grigolon,mgrigolon1a@cnet.com,7/26/2000
                Shannan,Rollason,srollason1b@cbslocal.com,2/11/2007
                Casie,Hefner,chefner1c@phoca.cz,11/28/2006
                King,Baynham,kbaynham1d@nba.com,11/12/2000";

            var users = userData.Split('\n');

            foreach (var user in users)
            {
                var userDetails = user.Split(',');
                var person = new Person(userDetails[0].Trim(), userDetails[1], userDetails[2], DateTime.Parse(userDetails[3]));
                await person.CalculateAdditionalFieldsAsync();

                await AddOrUpdatePerson(person);
            }
        }
    }
}
