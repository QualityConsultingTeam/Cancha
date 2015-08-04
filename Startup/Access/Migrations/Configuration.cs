using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using Access.Models;

namespace Access.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Access.AccessContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Access.AccessContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if(!context.Countries.Any()) Insertcountires(context);

            if(!context.Cities.Any())  InsertCities(context);

            if (!context.States.Any()) InsertStates(context);
        }

        #region Countries setup

        public void Insertcountires(AccessContext context)
        {
            var countries = new List<Country>()
            {
               new Country(1,"Afganist�n","AF","AFG","93",false),
new Country(2,"Albania","AL","ALB","355",false),
new Country(3,"Alemania","DE","DEU","49",false),
new Country(4,"Algeria","DZ","DZA","213",false),
new Country(5,"Andorra","AD","AND","376",false),
new Country(6,"Angola","AO","AGO","244",false),
new Country(7,"Anguila","AI","AIA","1 264",false),
new Country(8,"Ant�rtida","AQ","ATA","672",false),
new Country(9,"Antigua y Barbuda","AG","ATG","1 268",false),
new Country(10,"Antillas Neerlandesas","AN","ANT","599",false),
new Country(11,"Arabia Saudita","SA","SAU","966",false),
new Country(12,"Argentina","AR","ARG","54",false),
new Country(13,"Armenia","AM","ARM","374",false),
new Country(14,"Aruba","AW","ABW","297",false),
new Country(15,"Australia","AU","AUS","61",false),
new Country(16,"Austria","AT","AUT","43",false),
new Country(17,"Azerbaiy�n","AZ","AZE","994",false),
new Country(18,"B�lgica","BE","BEL","32",false),
new Country(19,"Bahamas","BS","BHS","1 242",false),
new Country(20,"Bahrein","BH","BHR","973",false),
new Country(21,"Bangladesh","BD","BGD","880",false),
new Country(22,"Barbados","BB","BRB","1 246",false),
new Country(23,"Belice","BZ","BLZ","501",false),
new Country(24,"Ben�n","BJ","BEN","229",false),
new Country(25,"But�n","BT","BTN","975",false),
new Country(26,"Bielorrusia","BY","BLR","375",false),
new Country(27,"Birmania","MM","MMR","95",false),
new Country(28,"Bolivia","BO","BOL","591",false),
new Country(29,"Bosnia y Herzegovina","BA","BIH","387",false),
new Country(30,"Botsuana","BW","BWA","267",false),
new Country(31,"Brasil","BR","BRA","55",false),
new Country(32,"Brun�i","BN","BRN","673",false),
new Country(33,"Bulgaria","BG","BGR","359",false),
new Country(34,"Burkina Faso","BF","BFA","226",false),
new Country(35,"Burundi","BI","BDI","257",false),
new Country(36,"Cabo Verde","CV","CPV","238",false),
new Country(37,"Camboya","KH","KHM","855",false),
new Country(38,"Camer�un","CM","CMR","237",false),
new Country(39,"Canad�","CA","CAN","1",false),
new Country(40,"Chad","TD","TCD","235",false),
new Country(41,"Chile","CL","CHL","56",false),
new Country(42,"China","CN","CHN","86",false),
new Country(43,"Chipre","CY","CYP","357",false),
new Country(44,"Ciudad del Vaticano","VA","VAT","39",false),
new Country(45,"Colombia","CO","COL","57",false),
new Country(46,"Comoras","KM","COM","269",false),
new Country(47,"Congo","CG","COG","242",false),
new Country(48,"Congo","CD","COD","243",false),
new Country(49,"Corea del Norte","KP","PRK","850",false),
new Country(50,"Corea del Sur","KR","KOR","82",false),
new Country(51,"Costa de Marfil","CI","CIV","225",false),
new Country(52,"Costa Rica","CR","CRI","506",false),
new Country(53,"Croacia","HR","HRV","385",false),
new Country(54,"Cuba","CU","CUB","53",false),
new Country(55,"Dinamarca","DK","DNK","45",false),
new Country(56,"Dominica","DM","DMA","1 767",false),
new Country(57,"Ecuador","EC","ECU","593",false),
new Country(58,"Egipto","EG","EGY","20",false),
new Country(59,"El Salvador","SV","SLV","503",true),
new Country(60,"Emiratos �rabes Unidos","AE","ARE","971",false),
new Country(61,"Eritrea","ER","ERI","291",false),
new Country(62,"Eslovaquia","SK","SVK","421",false),
new Country(63,"Eslovenia","SI","SVN","386",false),
new Country(64,"Espa�a","ES","ESP","34",false),
new Country(65,"Estados Unidos","US","USA","1",true),
new Country(66,"Estonia","EE","EST","372",false),
new Country(67,"Etiop�a","ET","ETH","251",false),
new Country(68,"Filipinas","PH","PHL","63",false),
new Country(69,"Finlandia","FI","FIN","358",false),
new Country(70,"Fiyi","FJ","FJI","679",false),
new Country(71,"Francia","FR","FRA","33",false),
new Country(72,"Gab�n","GA","GAB","241",false),
new Country(73,"Gambia","GM","GMB","220",false),
new Country(74,"Georgia","GE","GEO","995",false),
new Country(75,"Ghana","GH","GHA","233",false),
new Country(76,"Gibraltar","GI","GIB","350",false),
new Country(77,"Granada","GD","GRD","1 473",false),
new Country(78,"Grecia","GR","GRC","30",false),
new Country(79,"Groenlandia","GL","GRL","299",false),
new Country(80,"Guadalupe","GP","GLP","",false),
new Country(81,"Guam","GU","GUM","1 671",false),
new Country(82,"Guatemala","GT","GTM","502",true),
new Country(83,"Guayana Francesa","GF","GUF","",false),
new Country(84,"Guernsey","GG","GGY","",false),
new Country(85,"Guinea","GN","GIN","224",false),
new Country(86,"Guinea Ecuatorial","GQ","GNQ","240",false),
new Country(87,"Guinea-Bissau","GW","GNB","245",false),
new Country(88,"Guyana","GY","GUY","592",false),
new Country(89,"Hait�","HT","HTI","509",false),
new Country(90,"Honduras","HN","HND","504",true),
new Country(91,"Hong kong","HK","HKG","852",false),
new Country(92,"Hungr�a","HU","HUN","36",false),
new Country(93,"India","IN","IND","91",false),
new Country(94,"Indonesia","ID","IDN","62",false),
new Country(95,"Ir�n","IR","IRN","98",false),
new Country(96,"Irak","IQ","IRQ","964",false),
new Country(97,"Irlanda","IE","IRL","353",false),
new Country(98,"Isla Bouvet","BV","BVT","",false),
new Country(99,"Isla de Man","IM","IMN","44",false),
new Country(100,"Isla de Navidad","CX","CXR","61",false),
new Country(101,"Isla Norfolk","NF","NFK","",false),
new Country(102,"Islandia","IS","ISL","354",false),
new Country(103,"Islas Bermudas","BM","BMU","1 441",false),
new Country(104,"Islas Caim�n","KY","CYM","1 345",false),
new Country(105,"Islas Cocos (Keeling)","CC","CCK","61",false),
new Country(106,"Islas Cook","CK","COK","682",false),
new Country(107,"Islas de �land","AX","ALA","",false),
new Country(108,"Islas Feroe","FO","FRO","298",false),
new Country(109,"Islas Georgias del Sur y Sandwich del Sur","GS","SGS","",false),
new Country(110,"Islas Heard y McDonald","HM","HMD","",false),
new Country(111,"Islas Maldivas","MV","MDV","960",false),
new Country(112,"Islas Malvinas","FK","FLK","500",false),
new Country(113,"Islas Marianas del Norte","MP","MNP","1 670",false),
new Country(114,"Islas Marshall","MH","MHL","692",false),
new Country(115,"Islas Pitcairn","PN","PCN","870",false),
new Country(116,"Islas Salom?n","SB","SLB","677",false),
new Country(117,"Islas Turcas y Caicos","TC","TCA","1 649",false),
new Country(118,"Islas Ultramarinas Menores de Estados Unidos","UM","UMI","",false),
new Country(119,"Islas V�rgenes Brit�nicas","VG","VG","1 284",false),
new Country(120,"Islas V�rgenes de los Estados Unidos","VI","VIR","1 340",false),
new Country(121,"Israel","IL","ISR","972",false),
new Country(122,"Italia","IT","ITA","39",false),
new Country(123,"Jamaica","JM","JAM","1 876",false),
new Country(124,"Jap�n","JP","JPN","81",false),
new Country(125,"Jersey","JE","JEY","",false),
new Country(126,"Jordania","JO","JOR","962",false),
new Country(127,"Kazajist�n","KZ","KAZ","7",false),
new Country(128,"Kenia","KE","KEN","254",false),
new Country(129,"Kirguist�n","KG","KGZ","996",false),
new Country(130,"Kiribati","KI","KIR","686",false),
new Country(131,"Kuwait","KW","KWT","965",false),
new Country(132,"L�bano","LB","LBN","961",false),
new Country(133,"Laos","LA","LAO","856",false),
new Country(134,"Lesoto","LS","LSO","266",false),
new Country(135,"Letonia","LV","LVA","371",false),
new Country(136,"Liberia","LR","LBR","231",false),
new Country(137,"Libia","LY","LBY","218",false),
new Country(138,"Liechtenstein","LI","LIE","423",false),
new Country(139,"Lituania","LT","LTU","370",false),
new Country(140,"Luxemburgo","LU","LUX","352",false),
new Country(141,"M�xico","MX","MEX","52",false),
new Country(142,"M�naco","MC","MCO","377",false),
new Country(143,"Macao","MO","MAC","853",false),
new Country(144,"Maced�nia","MK","MKD","389",false),
new Country(145,"Madagascar","MG","MDG","261",false),
new Country(146,"Malasia","MY","MYS","60",false),
new Country(147,"Malawi","MW","MWI","265",false),
new Country(148,"Mali","ML","MLI","223",false),
new Country(149,"Malta","MT","MLT","356",false),
new Country(150,"Marruecos","MA","MAR","212",false),
new Country(151,"Martinica","MQ","MTQ","",false),
new Country(152,"Mauricio","MU","MUS","230",false),
new Country(153,"Mauritania","MR","MRT","222",false),
new Country(154,"Mayotte","YT","MYT","262",false),
new Country(155,"Micronesia","FM","FSM","691",false),
new Country(156,"Moldavia","MD","MDA","373",false),
new Country(157,"Mongolia","MN","MNG","976",false),
new Country(158,"Montenegro","ME","MNE","382",false),
new Country(159,"Montserrat","MS","MSR","1 664",false),
new Country(160,"Mozambique","MZ","MOZ","258",false),
new Country(161,"Namibia","NA","NAM","264",false),
new Country(162,"Nauru","NR","NRU","674",false),
new Country(163,"Nepal","NP","NPL","977",false),
new Country(164,"Nicaragua","NI","NIC","505",false),
new Country(165,"Niger","NE","NER","227",false),
new Country(166,"Nigeria","NG","NGA","234",false),
new Country(167,"Niue","NU","NIU","683",false),
new Country(168,"Noruega","NO","NOR","47",false),
new Country(169,"Nueva Caledonia","NC","NCL","687",false),
new Country(170,"Nueva Zelanda","NZ","NZL","64",false),
new Country(171,"Om�n","OM","OMN","968",false),
new Country(172,"Pa�ses Bajos","NL","NLD","31",false),
new Country(173,"Pakist�n","PK","PAK","92",false),
new Country(174,"Palau","PW","PLW","680",false),
new Country(175,"Palestina","PS","PSE","",false),
new Country(176,"Panam�","PA","PAN","507",false),
new Country(177,"Pap�a Nueva Guinea","PG","PNG","675",false),
new Country(178,"Paraguay","PY","PRY","595",false),
new Country(179,"Per�","PE","PER","51",false),
new Country(180,"Polinesia Francesa","PF","PYF","689",false),
new Country(181,"Polonia","PL","POL","48",false),
new Country(182,"Portugal","PT","PRT","351",false),
new Country(183,"Puerto Rico","PR","PRI","1",false),
new Country(184,"Qatar","QA","QAT","974",false),
new Country(185,"Reino Unido","GB","GBR","44",false),
new Country(186,"Rep�blica Centroafricana","CF","CAF","236",false),
new Country(187,"Rep�blica Checa","CZ","CZE","420",false),
new Country(188,"Rep�blica Dominicana","DO","DOM","1 809",false),
new Country(189,"Reuni�n","RE","REU","",false),
new Country(190,"Ruanda","RW","RWA","250",false),
new Country(191,"Rumania","RO","ROU","40",false),
new Country(192,"Rusia","RU","RUS","7",false),
new Country(193,"Sahara Occidental","EH","ESH","",false),
new Country(194,"Samoa","WS","WSM","685",false),
new Country(195,"Samoa Americana","AS","ASM","1 684",false),
new Country(196,"San Bartolom�","BL","BLM","590",false),
new Country(197,"San Crist�bal y Nieves","KN","KNA","1 869",false),
new Country(198,"San Marino","SM","SMR","378",false),
new Country(199,"San Mart�n (Francia)","MF","MAF","1 599",false),
new Country(200,"San Pedro y Miquel�n","PM","SPM","508",false),
new Country(201,"San Vicente y las Granadinas","VC","VCT","1 784",false),
new Country(202,"Santa Elena","SH","SHN","290",false),
new Country(203,"Santa Luc�a","LC","LCA","1 758",false),
new Country(204,"Santo Tom�s y Pr�ncipe","ST","STP","239",false),
new Country(205,"Senegal","SN","SEN","221",false),
new Country(206,"Serbia","RS","SRB","381",false),
new Country(207,"Seychelles","SC","SYC","248",false),
new Country(208,"Sierra Leona","SL","SLE","232",false),
new Country(209,"Singapur","SG","SGP","65",false),
new Country(210,"Siria","SY","SYR","963",false),
new Country(211,"Somalia","SO","SOM","252",false),
new Country(212,"Sri lanka","LK","LKA","94",false),
new Country(213,"Sud�frica","ZA","ZAF","27",false),
new Country(214,"Sud�n","SD","SDN","249",false),
new Country(215,"Suecia","SE","SWE","46",false),
new Country(216,"Suiza","CH","CHE","41",false),
new Country(217,"Surin�m","SR","SUR","597",false),
new Country(218,"Svalbard y Jan Mayen","SJ","SJM","",false),
new Country(219,"Swazilandia","SZ","SWZ","268",false),
new Country(220,"Tadjikist�n","TJ","TJK","992",false),
new Country(221,"Tailandia","TH","THA","66",false),
new Country(222,"Taiw�n","TW","TWN","886",false),
new Country(223,"Tanzania","TZ","TZA","255",false),
new Country(224,"Territorio Brit�nico del Oc�ano Idico","IO","IOT","",false),
new Country(225,"Territorios Australes y Ant�rticas Franceses","TF","ATF","",false),
new Country(226,"Timor Oriental","TL","TLS","670",false),
new Country(227,"Togo","TG","TGO","228",false),
new Country(228,"Tokelau","TK","TKL","690",false),
new Country(229,"Tonga","TO","TON","676",false),
new Country(230,"Trinidad y Tobago","TT","TTO","1 868",false),
new Country(231,"Tunez","TN","TUN","216",false),
new Country(232,"Turkmenist�n","TM","TKM","993",false),
new Country(233,"Turqu�a","TR","TUR","90",false),
new Country(234,"Tuvalu","TV","TUV","688",false),
new Country(235,"Ucrania","UA","UKR","380",false),
new Country(236,"Uganda","UG","UGA","256",false),
new Country(237,"Uruguay","UY","URY","598",false),
new Country(238,"Uzbekist�n","UZ","UZB","998",false),
new Country(239,"Vanuatu","VU","VUT","678",false),
new Country(240,"Venezuela","VE","VEN","58",false),
new Country(241,"Vietnam","VN","VNM","84",false),
new Country(242,"Wallis y Futuna","WF","WLF","681",false),
new Country(243,"Yemen","YE","YEM","967",false),
new Country(244,"Yibuti","DJ","DJI","253",false),
new Country(245,"Zambia","ZM","ZMB","260",false),
new Country(246,"Zimbabue","ZW","ZWE","263",false),

            };

            context.Countries.AddRange(countries);

            context.SaveChanges();
        }
        #endregion

        #region Insert Cities

        public void InsertCities(AccessContext context)
        {
            var cities = new List<City>()
            {
                new City(1, "Aguilares", 1),
                new City(2, "Apopa", 1),
                new City(3, "Ayutuxtepeque", 1),
                new City(4, "Delgado", 1),
                new City(5, "Cuscatancingo", 1),
                new City(6, "El Paisnal", 1),
                new City(7, "Guazapa", 1),
                new City(8, "Ilopango", 1),
                new City(9, "Mejicanos", 1),
                new City(10, "Nejapa", 1),
                new City(11, "Panchimalco", 1),
                new City(12, "Rosario de Mora", 1),
                new City(13, "San Marcos", 1),
                new City(14, "San Mar�n", 1),
                new City(15, "San Salvador", 1),
                new City(16, "Santiago Texacuangos", 1),
                new City(17, "Santo Tom�s", 1),
                new City(18, "Soyapango", 1),
                new City(19, "Tonacatepeque", 1),
                new City(20, "Ahuachap�n", 2),
                new City(21, "Apaneca", 2),
                new City(22, "Atiquizaya", 2),
                new City(23, "Concepci�n de Ataco", 2),
                new City(24, "El Refugio", 2),
                new City(25, "Guaymango", 2),
                new City(26, "Jujutla", 2),
                new City(27, "San Francisco Men�ndez", 2),
                new City(28, "San Lorenzo", 2),
                new City(29, "San Pedro Puxtla", 2),
                new City(30, "Tacuba", 2),
                new City(31, "Tur�n", 2),
                new City(32, "Acajutla", 3),
                new City(33, "Armenia", 3),
                new City(34, "Caluco", 3),
                new City(35, "Cuisnahuat", 3),
                new City(36, "Izalco", 3),
                new City(37, "Juay�a", 3),
                new City(38, "Nahuizalco", 3),
                new City(39, "Nahulingo", 3),
                new City(40, "Salcoatit�n", 3),
                new City(42, "San Antonio del Monte", 3),
                new City(43, "San Juli�n", 3),
                new City(45, "Santa Catarina Masahuat", 3),
                new City(46, "Santa Isabel Ishuat�n", 3),
                new City(48, "Santo Domingo de Guzm�n", 3),
                new City(49, "Sonsonate", 3),
                new City(50, " Sonzacate", 3),
                new City(51, "Candelaria de la Frontera", 4),
                new City(53, "Chalchuapa", 4),
                new City(54, "Coatepeque", 4),
                new City(55, "El Congo", 4),
                new City(56, "El Porvenir", 4),
                new City(57, "Masahuat", 4),
                new City(58, "Metap�n", 4),
                new City(59, "San Antonio Pajonal", 4),
                new City(60, "San Sebasti�n Salitrillo", 4),
                new City(61, "Santa Ana", 4),
                new City(62, "Santa Rosa Guachipil�n", 4),
                new City(63, "Santiago de la Frontera", 4),
                new City(64, "Texistepeque", 4),
                new City(65, "Cinquera", 5),
                new City(66, "Dolores / Villa Dolores", 5),
                new City(67, "Guacotecti", 5),
                new City(68, "Ilobasco", 5),
                new City(69, "Jutiapa", 5),
                new City(70, "San Isidro", 5),
                new City(71, "Sensuntepeque", 5),
                new City(72, "Tejutepeque", 5),
                new City(73, "Victoria", 5),
                new City(74, "Agua Caliente", 6),
                new City(75, "Arcatao", 6),
                new City(76, "Azacualpa", 6),
                new City(77, "Chalatenango", 6),
                new City(78, "Cital�", 6),
                new City(79, "Comalapa", 6),
                new City(80, "Concepci�n Quezaltepeque", 6),
                new City(81, "Dulce Nombre de Mar�a", 6),
                new City(82, "El Carrizal", 6),
                new City(83, "El Para�so", 6),
                new City(84, "La Laguna", 6),
                new City(85, "La Palma", 6),
                new City(86, "La Reina", 6),
                new City(87, "Las Vueltas", 6),
                new City(88, "Nombre de Jes�s", 6),
                new City(89, "Nueva Concepci{on", 6),
                new City(90, "Nueva Trinidad", 6),
                new City(91, "Ojos de Agua", 6),
                new City(92, "Potonico", 6),
                new City(93, "San Antonio de la Cruz", 6),
                new City(94, "San Antonio Los Ranchos", 6),
                new City(95, "San Fernando", 6),
                new City(96, "San Francisco Lempa", 6),
                new City(98, "San Francisco Moraz�n", 6),
                new City(99, "San Ignacio", 6),
                new City(100, "San Isidro Labrador", 6),
                new City(101, "San Jos� Cancasque / Cancasque", 6),
                new City(102, "San Jos� Las Flores / Las Flores", 6),
                new City(103, "San Luis del Carmen", 6),
                new City(104, "San Miguel de Mercedes", 6),
                new City(105, "San Rafael", 6),
                new City(106, "Santa Rita", 6),
                new City(107, "Tejutla", 6),
                new City(108, "Candelaria", 7),
                new City(109, "Cojutepeque", 7),
                new City(110, "El Carmen", 7),
                new City(111, "El Rosario", 7),
                new City(112, "Monte San Juan", 7),
                new City(113, "Oratorio de Concepci�n", 7),
                new City(114, "San Bartolom� Perulap�a", 7),
                new City(115, "San Crist�bal", 7),
                new City(116, "San Jos� Guayabal", 7),
                new City(117, "San Pedro Perulap�n", 7),
                new City(118, "San Rafael Cedros", 7),
                new City(119, "San Ram�n", 7),
                new City(120, "Santa Cruz Analquito", 7),
                new City(121, "Santa Cruz Michapa", 7),
                new City(122, "Suchitoto", 7),
                new City(123, "Tenancingo", 7),
                new City(124, "Antiguo Cuscatl�n", 8),
                new City(125, "Chiltiup�n", 8),
                new City(126, "Ciudad Arce", 8),
                new City(127, "Col�n", 8),
                new City(128, "Comasagua", 8),
                new City(129, "Huiz�car", 8),
                new City(130, "Jayaque", 8),
                new City(131, "Jicalapa", 8),
                new City(132, "La Libertad", 8),
                new City(133, "Santa Tecla", 8),
                new City(134, "Nuevo Cuscatl�n", 8),
                new City(135, "San Juan Opico", 8),
                new City(136, "Quezaltepeque", 8),
                new City(137, "Sacacoyo", 8),
                new City(138, "San Jos� Villanueva", 8),
                new City(139, "San Mat�as", 8),
                new City(140, "San Pablo Tacachico", 8),
                new City(141, "Talnique", 8),
                new City(142, "Tamanique", 8),
                new City(143, "Teotepeque", 8),
                new City(144, "Tepecoyo", 8),
                new City(145, "Zaragoza", 8),
                new City(146, "Cuyultit�n", 9),
                new City(147, "El Rosario / Rosario de La Paz", 9),
                new City(148, "Jerusal�n", 9),
                new City(149, "Mercedes La Ceiba", 9),
                new City(150, "Olocuilta", 9),
                new City(151, "Para�so de Osorio", 9),
                new City(152, "San Antonio Masahuat", 9),
                new City(153, "San Emigdio", 9),
                new City(154, "San Francisco Chinameca", 9),
                new City(155, "San Juan Nonualco", 9),
                new City(156, "San Juan Talpa", 9),
                new City(157, "San Juan Tepezontes", 9),
                new City(158, "San Luis La Herradura", 9),
                new City(159, "San Luis Talpa", 9),
                new City(160, "San Miguel Tepezontes", 9),
                new City(161, "San Pedro Masahuat", 9),
                new City(162, "San Pedro Nonualco", 9),
                new City(163, "San Rafael Obrajuelo", 9),
                new City(164, "Santa Mar�a Ostuma", 9),
                new City(165, "Santiago Nonualco", 9),
                new City(166, "Tapalhuaca", 9),
                new City(167, "Zacatecoluca", 9),
                new City(168, "Apastepeque", 10),
                new City(169, "Guadalupe", 10),
                new City(170, "San Cayetano Istepeque", 10),
                new City(171, "San Esteban Catarina", 10),
                new City(172, "San Ildefonso", 10),
                new City(174, "San Lorenzo", 10),
                new City(175, "San Sebasti�n", 10),
                new City(176, "San Vicente", 10),
                new City(177, "Santa Clara", 10),
                new City(178, "Santo Domingo", 10),
                new City(179, "Tecoluca", 10),
                new City(180, "Tepetit�n", 10),
                new City(181, "Verapaz", 10),
                new City(182, "Arambala", 11),
                new City(183, "Cacaopera", 11),
                new City(184, "Chilanga", 11),
                new City(185, "Corinto", 11),
                new City(186, "Delicias de Concepci�n", 11),
                new City(187, "El Divisadero", 11),
                new City(188, "El Rosario", 11),
                new City(189, "Gualococti", 11),
                new City(190, "Guatajiagua", 11),
                new City(191, "Joateca", 11),
                new City(192, "Jocoaitique", 11),
                new City(193, "Jocoro", 11),
                new City(194, "Lolotiquillo", 11),
                new City(195, "Meanguera", 11),
                new City(196, "Osicala", 11),
                new City(197, "Perqu�n", 11),
                new City(198, "San Carlos", 11),
                new City(199, "San Fernando", 11),
                new City(200, "San Francisco Gotera", 11),
                new City(201, "San Isidro", 11),
                new City(202, "San Sim�n", 11),
                new City(203, "Sensembra", 11),
                new City(204, "Sociedad", 11),
                new City(205, "Torola", 11),
                new City(206, "Yamabal", 11),
                new City(207, "Yoloaiqu�n", 11),
                new City(208, "Carolina", 12),
                new City(209, "Chapeltique", 12),
                new City(210, "Chinameca", 12),
                new City(211, "Chirilagua", 12),
                new City(212, "Ciudad Barrios", 12),
                new City(213, "Comacar�n", 12),
                new City(214, "El Tr�nsito", 12),
                new City(215, "Lolotique", 12),
                new City(216, "Moncagua", 12),
                new City(217, "Nueva Guadalupe", 12),
                new City(218, "Nuevo Ed�n de San Juan", 12),
                new City(220, "Quelepa", 12),
                new City(221, "San Antonio del Mosco", 12),
                new City(222, "San Gerardo", 12),
                new City(223, "San Jorge", 12),
                new City(224, "San Luis de la Reina", 12),
                new City(225, "San Miguel", 12),
                new City(226, "San Rafael Oriente", 12),
                new City(227, "Sesori", 12),
                new City(228, "Uluazapa", 12),
                new City(229, "Alegr�a", 13),
                new City(230, "Berl�n", 13),
                new City(231, "California", 13),
                new City(232, "Concepci�n Batres", 13),
                new City(233, "El Triunfo", 13),
                new City(234, "Ereguayqu�n", 13),
                new City(235, "Estanzuelas", 13),
                new City(236, "Jiquilisco", 13),
                new City(237, "Jucuapa", 13),
                new City(238, "Jucuar�n", 13),
                new City(239, "Mercedes Uma�a", 13),
                new City(240, "Nueva Granada", 13),
                new City(241, "Ozatl�n", 13),
                new City(242, "Puerto El Triunfo", 13),
                new City(243, "San Agust�n", 13),
                new City(244, "San Buenaventura", 13),
                new City(245, "San Dionisio", 13),
                new City(246, "San Francisco Javier", 13),
                new City(247, "Santa Elena", 13),
                new City(248, "Santa Mar�a", 13),
                new City(249, "Santiago de Mar�a", 13),
                new City(250, "Tecap�n", 13),
                new City(251, "Usulut�n", 13),
                new City(252, "Anamor�s", 14),
                new City(253, "Bol�var", 14),
                new City(254, "Concepci�n de Oriente", 14),
                new City(255, "Conchagua", 14),
                new City(256, "El Carmen", 14),
                new City(257, "El Sauce", 14),
                new City(258, "Intipuc�", 14),
                new City(259, "La Uni�n", 14),
                new City(260, "Lilisque", 14),
                new City(261, "Meanguera del Golfo", 14),
                new City(262, "Nueva Esparta", 14),
                new City(263, "Pasaquina", 14),
                new City(265, "Polor�s", 14),
                new City(266, "San Alejo", 14),
                new City(267, "San Jos�", 14),
                new City(268, "Santa Rosa de Lima", 14),
                new City(269, "Yayantique", 14),
                new City(270, "Yucuaiqu�n", 14),
                new City(1002, "Montgomery", 1002),
                new City(1003, "Juneau", 1005),
                new City(1004, "Phoenix", 1006),
                new City(1005, "Little Rock", 1007),
                new City(1006, "Sacramento", 1008),
                new City(1007, "Denver", 1009),
                new City(1008, "Hartford", 1010),
                new City(1009, "Dover", 1011),
                new City(1010, "Tallahassee", 1012),
                new City(1011, "Atlanta", 1013),
                new City(1012, "Honolulu", 1014),
                new City(1013, "Boise", 1015),
                new City(1014, "Springfield", 1016),
                new City(1015, "Indianapolis", 1017),
                new City(1016, "Des Moines", 1018),
                new City(1017, "Topeka", 1019),
                new City(1018, "Frankfort", 1020),
                new City(1019, "Baton Rouge", 1021),
                new City(1020, "Augusta", 1022),
                new City(1021, "Annapolis", 1023),
                new City(1022, "Boston", 1024),
                new City(1023, "Lansing", 1025),
                new City(1024, "Saint Paul", 1026),
                new City(1025, "Jackson", 1027),
                new City(1026, "Jefferson City", 1028),
                new City(1027, "Helena", 1029),
                new City(1028, "Lincoln", 1030),
                new City(1029, "Carson City", 1031),
                new City(1030, "Concord", 1032),
                new City(1031, "Trenton", 1033),
                new City(1032, "Santa Fe", 1034),
                new City(1033, "Albany (New York)", 1035),
                new City(1034, "Raleigh", 1036),
                new City(1035, "Bismarck", 1037),
                new City(1036, "Columbus", 1038),
                new City(1037, "Oklahoma City", 1039),
                new City(1038, "Salem", 1040),
                new City(1039, "Harrisburg", 1041),
                new City(1040, "Providence", 1042),
                new City(1041, "Columbia", 1043),
                new City(1042, "Pierre", 1044),
                new City(1043, "Nashville", 1045),
                new City(1044, "Austin", 1046),
                new City(1045, "Salt Lake City", 1047),
                new City(1046, "Montpelier", 1048),
                new City(1047, "Richmond", 1049),
                new City(1048, "Olympia", 1050),
                new City(1049, "Charleston", 1051),
                new City(1050, "Madison", 1052),
                new City(1059, "Cheyenne", 1056),

            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
        #endregion

        #region Insert States

        public void InsertStates(AccessContext context)
        {
            var states = new List<States>()
            {
                new States(1,"San Salvador",59),
new States(2,"Ahuachap�n",59),
new States(3,"Sonsonate",59),
new States(4,"Santa Ana",59),
new States(5,"Caba�as",59),
new States(6,"Chalatenango",59),
new States(7,"Cuscatl�n",59),
new States(8,"La Libertad",59),
new States(9,"La Paz",59),
new States(10,"San Vicente",59),
new States(11,"Moraz�n",59),
new States(12,"San Miguel",59),
new States(13,"Usulut�n",59),
new States(14,"La Uni�n",59),
new States(1002,"Alabama",65),
new States(1005,"Alaska",65),
new States(1006,"Arizona",65),
new States(1007,"Arkansas",65),
new States(1008,"California",65),
new States(1009,"Colorado",65),
new States(1010,"Connecticut",65),
new States(1011,"Delaware",65),
new States(1012,"Florida",65),
new States(1013,"Georgia",65),
new States(1014,"Haw�i",65),
new States(1015,"Idaho",65),
new States(1016,"Illinois",65),
new States(1017,"Indiana",65),
new States(1018,"Iowa",65),
new States(1019,"Kansas",65),
new States(1020,"Kentucky",65),
new States(1021,"Louisiana",65),
new States(1022,"Maine",65),
new States(1023,"Maryland",65),
new States(1024,"Massachusetts",65),
new States(1025,"M�chigan",65),
new States(1026,"Minnesota",65),
new States(1027,"Mississippi",65),
new States(1028,"Missouri",65),
new States(1029,"Montana",65),
new States(1030,"Nebraska",65),
new States(1031,"Nevada",65),
new States(1032,"New Hampshire",65),
new States(1033,"New Jersey",65),
new States(1034,"New Mexico",65),
new States(1035,"Nueva York",65),
new States(1036,"North Carolina",65),
new States(1037,"North Dakota",65),
new States(1038,"Ohio",65),
new States(1039,"Oklahoma",65),
new States(1040,"Oreg�n",65),
new States(1041,"Pennsylvania",65),
new States(1042,"Rhode Island",65),
new States(1043,"South Carolina",65),
new States(1044,"South Dakota",65),
new States(1045,"Tennessee",65),
new States(1046,"Texas",65),
new States(1047,"Utah",65),
new States(1048,"Vermont",65),
new States(1049,"Virginia",65),
new States(1050,"Washington",65),
new States(1051,"West Virginia",65),
new States(1052,"Wisconsin",65),
new States(1056,"Wyoming",65),
new States(1057,"No especificado",59),

            };

            context.States.AddRange(states);

            context.SaveChanges();
        }
        #endregion
    }
}
