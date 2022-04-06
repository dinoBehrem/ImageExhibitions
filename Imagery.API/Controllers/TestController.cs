using Imagery.Service.Services.Authentication;
using Imagery.Service.Services.Exhbition;
using Imagery.Service.Services.Image;
using Imagery.Service.Services.Topics;
using Imagery.Service.ViewModels.Exhbition;
using Imagery.Service.ViewModels.Image;
using Imagery.Service.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imagery.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IExhibitionService exhibitionService;
        private readonly IImageService imageService;
        private readonly ITopicService topicService;
        private readonly IAuthService authService;

        const string loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        const string titles = "At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga.Et harum quidem rerum facilis est et expedita distinctio.Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae.Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat.";

        List<string> profilePictures = new List<string>()
            {
                "https://localhost:44395/ProfilePictures/pexels-creation-hill-16810108df4261f-f022-4126-b3f3-c916841ef058.jpg",
                "https://localhost:44395/ProfilePictures/pexels-fauzan-muzakky-5010780bc4e308e-4e10-4085-92ba-5f17dd6c0df8.jpg",
                "https://localhost:44395/ProfilePictures/pexels-gu-kssn-114524646c45f4e6-e4a8-464a-b3b4-eda516dde177.jpg",
                "https://localhost:44395/ProfilePictures/pexels-lola-russian-18555823e84243c-9f8a-4d1c-9399-b60f68cff81b.jpg",
                "https://localhost:44395/ProfilePictures/pexels-nathan-cowley-130040292a90f04-f754-48e4-a475-fadf494912bb.jpg",
                "https://localhost:44395/ProfilePictures/pexels-nguyen-hung-114496354ddc8a90-c412-4a65-9f7c-dd9df3c48e34.jpg",
                "https://localhost:44395/ProfilePictures/pexels-pixabay-350657c4fc270-acea-42ab-bfd4-c3c0f51340d6.jpg",
                "https://localhost:44395/ProfilePictures/pexels-pixabay-38554cad3411c-e823-44a0-9548-e7d7953e7042.jpg",
                "https://localhost:44395/ProfilePictures/pexels-pixabay-1416514fdf37b0-e9ef-4e0f-aeb5-37753759467e.jpg",
                "https://localhost:44395/ProfilePictures/pexels-pixabay-22045377df66bf-84d5-4b66-bb4b-17a388749281.jpg",
                "https://localhost:44395/ProfilePictures/pexels-ralph-rabago-3214729ce9530ed-33e6-43a4-b046-ed0d41880630.jpg",
                "https://localhost:44395/ProfilePictures/pexels-simon-robben-614810752f0d6a-502f-4ca7-9271-bc8cc35c47f9.jpg",
                "https://localhost:44395/ProfilePictures/pexels-sinitta-leunen-50142132f6c2711-8b32-44b9-aedd-4d7c7a2cd068.jpg",
                "https://localhost:44395/ProfilePictures/pexels-spring-toan-4075524fb9be65e-9dce-4e83-af1c-ba6c59a42738.jpg",
                "https://localhost:44395/ProfilePictures/pexels-jc-siller-6948652d04b8ce5-b213-4106-8c41-826e76b7d765.jpg",
                "https://localhost:44395/ProfilePictures/profilePicPlaceholder4cd83466-ce97-47e3-a3b2-dc6b2bc2085a.png"
            };
        List<string> exponentItems = new List<string>()
            {
                "https://localhost:44395/ExponentItems/pexels-alexander-ant-7004697aaf6abc0-4217-4619-8f05-b02a67c3487f.jpg",
"https://localhost:44395/ExponentItems/pexels-ali-şenol-933443402f741c5-a074-47a1-a0ed-ae292678a52a.jpg",
"https://localhost:44395/ExponentItems/pexels-anni-roenkae-417505491ab273d-ce3c-4448-abbc-48abe3c97114.jpg",
"https://localhost:44395/ExponentItems/pexels-daria-liudnaya-735476005fea98b-da06-4cf4-bba7-2c795f69bafb.jpg",
"https://localhost:44395/ExponentItems/pexels-david-selbert-8797307dafef748-c0e8-447e-bbe4-85026078f123.jpg",
"https://localhost:44395/ExponentItems/pexels-egor-kamelev-927497ecda291a-2273-4ce0-8ba0-dafb1c140ed3.jpg",
"https://localhost:44395/ExponentItems/pexels-evie-shaffer-2512282c4916293-36c0-414e-a601-3574c0fcce4a.jpg",
"https://localhost:44395/ExponentItems/pexels-hakeem-james-hausley-40902695fcd0944-1936-4502-a624-ad22f8eb0a9f.jpg",
"https://localhost:44395/ExponentItems/pexels-huzni-mhmd-95676983db42c7-c26c-4b75-987c-0aad84cf4235.jpg",
"https://localhost:44395/ExponentItems/pexels-jan-747002710d76ec2-058c-4107-9472-a80b3146c829.jpg",
"https://localhost:44395/ExponentItems/pexels-jean-van-der-meulen-15434176bc99c81-e60a-4c3a-bff1-0f5430177264.jpg",
"https://localhost:44395/ExponentItems/pexels-jeremy-bishop-2422915be66c4fd-6582-4adb-ad86-3c58d8e8f15d.jpg",
"https://localhost:44395/ExponentItems/pexels-jiarong-deng-10346621c328c0a-2e82-4788-80bb-bd6964f073b7.jpg",
"https://localhost:44395/ExponentItems/pexels-luis-dalvan-17708099dae343f-6d15-4edc-9c56-c72deb3858f7.jpg",
"https://localhost:44395/ExponentItems/pexels-lukas-rodriguez-56184636e5e9c10-1a0f-4901-92ba-1532f089ba54.jpg",
"https://localhost:44395/ExponentItems/pexels-nadi-lindsay-498287804d5477c-6b62-4150-ba43-5fe79e6318c7.jpg",
"https://localhost:44395/ExponentItems/pexels-oliver-sjöström-1122414e1c7a835-5f35-432d-b8f1-1a274fc68755.jpg",
"https://localhost:44395/ExponentItems/pexels-pavel-danilyuk-6925162018fdf95-bdb5-4a88-8650-246f49cc7d89.jpg",
"https://localhost:44395/ExponentItems/pexels-pineapple-supply-co-13707749adfd11-e9cf-4473-b6b0-99e36fa76a81.jpg",
"https://localhost:44395/ExponentItems/pexels-ricardo-esquivel-15632560e03c3d0-fc74-417f-83b3-c927eb19df90.jpg",
"https://localhost:44395/ExponentItems/pexels-sem-steenbergen-36213440391e9fe-62f6-4c89-ac4e-9528e94ec698.jpg",
"https://localhost:44395/ExponentItems/pexels-s-migaj-1402850ad872e9f-2755-4920-a485-2a00d728a88c.jpg",
"https://localhost:44395/ExponentItems/pexels-stein-egil-liland-340874452791705-a477-48f3-b455-f3ab4e6a1abc.jpg",
"https://localhost:44395/ExponentItems/pexels-steve-johnson-84524215459c35-3166-464d-9ac2-5249593cb53b.jpg",
"https://localhost:44395/ExponentItems/pexels-steve-johnson-31896070f7e22f4-4089-410b-8b84-8fcfc25df789.jpg",
"https://localhost:44395/ExponentItems/pexels-susanne-jutzeler-70181410a291fe9-8412-4948-8c65-92324c493bd7.jpg",
"https://localhost:44395/ExponentItems/pexels-tayla-walsh-9486201cb459d9-b7d5-4c9e-92cd-9ad54e5c9d7d.jpg",
"https://localhost:44395/ExponentItems/pexels-thirdman-67326585eafa59d-71ac-4781-875e-360d4e651d04.jpg",
"https://localhost:44395/ExponentItems/pexels-torsten-dettlaff-971546bdf31b12-b06b-4b0a-9ed6-4d9ccaa97d9d.jpg",
"https://localhost:44395/ExponentItems/pexels-walid-ahmad-15095829fe5f191-86c4-4434-935c-93ad2a6a79eb.jpg"
            };
        List<string> roles = new List<string>()
        {
            "User",
            "Admin",
            "SuperAdmin"
        };
        List<string> topics = new List<string>()
            {
                "Digital art",
                "Drawings & Paintings",
                "Photography",
                "3D Model",
                "2D Model",
                "Sculpting",
                "Abstract",
                "Animals & Wildlife",
                "Fantasy",
                "Portraits",
                "Oil painting",
                "Watercolor",
                "Colored pencils"
            };
        List<string> dimensions = new List<string>()
            {
                "20x15",
                "22x17",
                "24x19",
                "30x25",
                "35x28",
                "40x30",
                "50x40",
                "70x55"
            };
        List<string> phoneNumbers = new List<string>()
            {
                "697-555-0142",
                "819-555-0175",
                "212-555-0187",
                "612-555-0100",
                "849-555-0139",
                "122-555-0189",
                "181-555-0156",
                "815-555-0138",
                "185-555-0186",
                "330-555-2568",
                "719-555-0181",
                "168-555-0183",
                "473-555-0117",
                "465-555-0156",
                "970-555-0138",
                "913-555-0172"
            };
            
        List<RegisterVM> users = new List<RegisterVM>()
            {
                new RegisterVM() { Firstname = "Michael", Lastname = "O'Leary", Email = "michael@email.com", Username = "Michael", Password = "Michael1" },
                new RegisterVM() { Firstname = "Fauzan", Lastname = "Muzzaky", Email = "fauzan@email.com", Username = "Fauzan", Password = "Fauzan12" },
                new RegisterVM() { Firstname = "Cheryl", Lastname = "Carson", Email = "cheryl@email.com", Username = "Cheryl", Password = "Cheryl12" },
                new RegisterVM() { Firstname = "Meander", Lastname = "Smith", Email = "meander@email.com", Username = "Meander", Password = "Meander1" },
                new RegisterVM() { Firstname = "Johnson", Lastname = "White", Email = "johnson@email.com", Username = "Johnson", Password = "Johnson1" },
                new RegisterVM() { Firstname = "Akiko", Lastname = "Yokomoto", Email = "akiko@email.com", Username = "Akiko", Password = "Akiko123" },
                new RegisterVM() { Firstname = "Dean", Lastname = "Straight", Email = "dean@email.com", Username = "Dean", Password = "Dean1234" },
                new RegisterVM() { Firstname = "Innes", Lastname = "del Castillo", Email = "innes@email.com", Username = "Innes", Password = "Innes123" },
                new RegisterVM() { Firstname = "Burt", Lastname = "Gringlesby", Email = "burt@email.com", Username = "Burt", Password = "Burt1234" },
                new RegisterVM() { Firstname = "Albert", Lastname = "Ringer", Email = "albert@email.com", Username = "Albert", Password = "Albert12" },
                new RegisterVM() { Firstname = "Livia", Lastname = "Karsen", Email = "livia@email.com", Username = "Livia", Password = "Livia123" },
                new RegisterVM() { Firstname = "Reginald", Lastname = "Blotchet-Halls", Email = "reginald@email.com", Username = "Reginald", Password = "Reginald1" },
                new RegisterVM() { Firstname = "Charlene", Lastname = "Locksley", Email = "charlene@email.com", Username = "Charlene", Password = "Charlene1" },
                new RegisterVM() { Firstname = "Sylvia", Lastname = "Panteley", Email = "sylvia@email.com", Username = "Sylvia", Password = "Sylvia12" },
                new RegisterVM() { Firstname = "Marjorie", Lastname = "Green", Email = "marjorie@email.com", Username = "Marjorie", Password = "Marjorie1" },
                new RegisterVM() { Firstname = "SuperAdmin", Lastname = "SuperAdmin", Email = "superAdmin@email.com", Username = "SuperAdmin", Password = "SuperAdmin1" },
            };

        public TestController(IUserService userService, IExhibitionService exhibitionService, IImageService imageService, ITopicService topicService, IAuthService authService)
        {
            this.userService = userService;
            this.exhibitionService = exhibitionService;
            this.imageService = imageService;
            this.topicService = topicService;
            this.authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult> GenerateData()
        {
            var rand = new Random();

            // Generate user roles

            if (authService.GetRoles().Count == 0)
            {
                foreach (var role in roles)
                {
                    await authService.CreateRole(role);
                }
            }

           
            // Generate exhibition topic tags

            if (topicService.GetAllTopics().Count == 0)
            {
                foreach (var topic in topics)
                {
                    topicService.Create(topic);
                }
            }

           
            // Generate users

            await userService.AddTestUsers(users, profilePictures);
            await userService.TestSubscriptions(users);
            await userService.SuperAdminMethod("SuperAdmin");

            string phone;
            string bio;

            for (int i = 0; i < users.Count; i++)
            {
                phone = phoneNumbers[i];
                bio = titles.Substring(rand.Next(0, 611), 150);
                await userService.EditUserTest(users[i], bio, phone);
            }


            // Generate exhibitions

            HashSet<DimensionsVM> itemDimensions = new HashSet<DimensionsVM>();

            foreach (var user in users)
            {
                DateTime date = DateTime.Now;

                int exhibitionCount = rand.Next(2, 4);

                if (user.Username != Roles.SuperAdmin)
                {
                    for (int i = 0; i < exhibitionCount; i++)
                    {
                        int days = rand.Next(0, 15);
                        int hours = rand.Next(0, 24);
                        int minutes = rand.Next(0, 60);

                        date = date.AddDays(days);
                        date = date.AddHours(hours);
                        date = date.AddMinutes(minutes);

                        var exhbitionCreation = new ExhbitionCreationVM()
                        {
                            Organizer = user.Username,
                            Title = loremIpsum.Substring(rand.Next(0, 420), 25),
                            Description = titles.Substring(rand.Next(0, 671), 170),
                            StartingDate = date
                        };

                        int id = await exhibitionService.AddTestExhibitions(exhbitionCreation, users);

                        int itemsCount = rand.Next(2, 5);

                        for (int j = 0; j < itemsCount; j++)
                        {
                            int dimensionsCount = rand.Next(1, 3);

                            for (int k = 0; k < dimensionsCount; k++)
                            {
                                itemDimensions.Add(new DimensionsVM()
                                {
                                    Dimension = dimensions[rand.Next(0, dimensions.Count)],
                                    Price = rand.Next(10, 200)
                                });
                            }

                            exhibitionService.TestItems(id, new TestItemUploadVM()
                            {
                                Name = loremIpsum.Substring(rand.Next(0, 420), 25),
                                Creator = user.Firstname + " " + user.Lastname,
                                Description = titles.Substring(rand.Next(0, 711), 50),
                                Image = exponentItems[rand.Next(0, exponentItems.Count)]
                            }, itemDimensions.ToList());

                            itemDimensions.Clear();
                        }
                    }
                }
            }

            //string link = imageService.TestItemUpload(profile.Image);

            return Ok("Data successfully generated!");
        }
       
    }
}
