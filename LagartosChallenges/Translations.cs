using System.Collections.Generic;

namespace LagartosChallenges
{
    /// <summary>
    /// Every text the mod shows in game, in English plus the 11 other languages the base game ships.
    /// The game translates by looking up the English text (<c>Localization.Translate</c>), so the English
    /// constants are both what the mod registers and the lookup keys; <see cref="Plugin"/> feeds
    /// <see cref="All"/> to InscryptionAPI's <c>LocalizationManager</c>.
    /// Wording follows the game's own translations (e.g. "Squirrel", "sigil", "Annoying", the pelt names).
    /// Kept free of Unity/game types so it can be unit tested.
    /// </summary>
    public static class Translations
    {
        /// <summary>Unfair Hand challenge title. Also part of the challenge id, so never change it.</summary>
        public const string UnfairHandTitle = "Unfair Hand";

        /// <summary>Unfair Hand challenge description.</summary>
        public const string UnfairHandDescription = "Your opening hand is no longer guaranteed to be playable.";

        /// <summary>Worthy Squirrel challenge title. Also part of the challenge id, so never change it.</summary>
        public const string WorthySquirrelTitle = "Worthy Squirrel";

        /// <summary>Worthy Squirrel challenge description (phrased like the game's "Squirrel Fish").</summary>
        public const string WorthySquirrelDescription = "Your opening squirrel has the Double Blood sigil.";

        /// <summary>Sigil rulebook name. Also part of the sigil id, so never change it.</summary>
        public const string DoubleBloodName = "Double Blood";

        /// <summary>
        /// Sigil rulebook description (phrased like the game's Worthy Sacrifice). The game replaces
        /// <c>[creature]</c> with its own translation of "a card bearing this sigil".
        /// </summary>
        public const string DoubleBloodDescription = "[creature] is counted as 2 Blood rather than 1 Blood when sacrificed.";

        /// <summary>Annoying Trades challenge title. Also part of the challenge id, so never change it.</summary>
        public const string AnnoyingTradesTitle = "Annoying Trades";

        /// <summary>Annoying Trades challenge description.</summary>
        public const string AnnoyingTradesDescription =
            "Annoying Starters also affects pelts. At every trade, if any of your pelts is Annoying, all offered cards are too.";

        /// <summary>
        /// English text → (InscryptionAPI language code → translated text).
        /// Codes: fr, it, de, es, pt, tr, ru, ja, ko, zhcn, zhtw.
        /// </summary>
        public static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> All =
            new Dictionary<string, IReadOnlyDictionary<string, string>>
            {
                [UnfairHandTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Main injuste",
                    ["it"] = "Mano ingiusta",
                    ["de"] = "Unfaire Hand",
                    ["es"] = "Mano injusta",
                    ["pt"] = "Mão Injusta",
                    ["tr"] = "Adaletsiz El",
                    ["ru"] = "Нечестная рука",
                    ["ja"] = "不公平な手札",
                    ["ko"] = "불공평한 패",
                    ["zhcn"] = "不公平手牌",
                    ["zhtw"] = "不公平手牌",
                },
                [UnfairHandDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Votre main de départ n'est plus forcément jouable.",
                    ["it"] = "Non è più garantito che la tua mano iniziale sia giocabile.",
                    ["de"] = "Deine Starthand ist nicht mehr garantiert spielbar.",
                    ["es"] = "Ya no se garantiza que tu mano inicial sea jugable.",
                    ["pt"] = "Sua mão inicial não tem mais garantia de ser jogável.",
                    ["tr"] = "Başlangıç elinin oynanabilir olması artık garanti değil.",
                    ["ru"] = "Ваша стартовая рука больше не обязательно будет играбельной.",
                    ["ja"] = "最初の手札が使えるものになる保証はもうない。",
                    ["ko"] = "시작 패를 낼 수 있다는 보장이 더 이상 없다.",
                    ["zhcn"] = "起始手牌不再保证可以打出。",
                    ["zhtw"] = "起始手牌不再保證可以打出。",
                },
                [WorthySquirrelTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Écureuil digne",
                    ["it"] = "Degno scoiattolo",
                    ["de"] = "Würdiges Eichhörnchen",
                    ["es"] = "Ardilla digna",
                    ["pt"] = "Esquilo Digno",
                    ["tr"] = "Kutsal Sincap",
                    ["ru"] = "Достойная белка",
                    ["ja"] = "価値あるリス",
                    ["ko"] = "값진 다람쥐",
                    ["zhcn"] = "优质松鼠",
                    ["zhtw"] = "夠格松鼠",
                },
                [WorthySquirrelDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Votre écureuil de départ a l'emblème Double sang.",
                    ["it"] = "Il tuo scoiattolo iniziale ha il sigillo Doppio sangue.",
                    ["de"] = "Dein Start-Eichhörnchen hat das Siegel „Doppelblut“.",
                    ["es"] = "Tu Ardilla inicial tiene el sello Sangre doble.",
                    ["pt"] = "Seu Esquilo inicial recebe o selo Sangue Duplo.",
                    ["tr"] = "Başlangıç sincabın Çift Kan mührüne sahip olur.",
                    ["ru"] = "Ваша стартовая белка получит символ «Двойная кровь».",
                    ["ja"] = "最初のリスに「二倍の血」の印がつく。",
                    ["ko"] = "시작 다람쥐에게 두 배의 피 도장이 주어진다.",
                    ["zhcn"] = "你的起始松鼠带有双倍血量印记。",
                    ["zhtw"] = "你的起始松鼠會有雙倍鮮血印記。",
                },
                [DoubleBloodName] = new Dictionary<string, string>
                {
                    ["fr"] = "Double sang",
                    ["it"] = "Doppio sangue",
                    ["de"] = "Doppelblut",
                    ["es"] = "Sangre doble",
                    ["pt"] = "Sangue Duplo",
                    ["tr"] = "Çift Kan",
                    ["ru"] = "Двойная кровь",
                    ["ja"] = "二倍の血",
                    ["ko"] = "두 배의 피",
                    ["zhcn"] = "双倍血量",
                    ["zhtw"] = "雙倍鮮血",
                },
                [DoubleBloodDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Sacrifier [creature] accorde 2 gouttes de sang au lieu de 1.",
                    ["it"] = "Quando viene sacrificata, [creature] vale 2 di sangue invece di 1.",
                    ["de"] = "[creature] bringt beim Opfern 2 statt 1 Blut ein.",
                    ["es"] = "Cuando se sacrifica, [creature] se cuenta como 2 de sangre en lugar de 1 de sangre.",
                    ["pt"] = "[creature] concede 2 de sangue (ao invés de 1) ao ser sacrificada.",
                    ["tr"] = "[creature], kurban edildiğinde 1 yerine 2 Kan verir.",
                    ["ru"] = "Когда [creature] приносится в жертву, вы получаете 2 капли крови вместо одной.",
                    ["ja"] = "[creature]を捧げると、血が1個ではなく2個手に入る。",
                    ["ko"] = "[creature]은(는) 희생될 시 1이 아닌 2의 피를 제공한다.",
                    ["zhcn"] = "[creature]在献祭时可算作2点血量，而非1点。",
                    ["zhtw"] = "[creature] 被犧牲時視為 2 點鮮血，而非 1 點。",
                },
                [AnnoyingTradesTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Échanges Fureur",
                    ["it"] = "Scambi irritanti",
                    ["de"] = "Nervige Tauschgeschäfte",
                    ["es"] = "Trueques molestos",
                    ["pt"] = "Trocas Irritantes",
                    ["tr"] = "Sinir Bozucu Takaslar",
                    ["ru"] = "Надоедливый обмен",
                    ["ja"] = "「嫌がらせ」の取引",
                    ["ko"] = "구타유발자 거래",
                    ["zhcn"] = "惹人生厌的交易",
                    ["zhtw"] = "煩躁交易",
                },
                [AnnoyingTradesDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "« Fureur du défi » touche aussi les fourrures. À chaque échange, si une de vos fourrures a Fureur, toutes les cartes proposées l'ont aussi.",
                    ["it"] = "«Carte iniziali irritanti» vale anche per le pelli. A ogni scambio, se una tua pelle è Irritante, lo sono anche tutte le carte offerte.",
                    ["de"] = "„Nervige Startkarten“ gilt auch für Felle. Bei jedem Tausch: Ist eines deiner Felle nervig, sind es alle angebotenen Karten auch.",
                    ["es"] = "«Molestias iniciales» también afecta a las pieles. En cada trueque, si alguna de tus pieles es Molesta, todas las cartas ofrecidas también.",
                    ["pt"] = "\"Irritação inicial\" também afeta as peles. Em toda troca, se alguma das suas peles for Irritante, todas as cartas oferecidas também serão.",
                    ["tr"] = "\"Sinir Bozucu Başlangıç Destesi\" postları da etkiler. Her takasta postlarından biri Sinir Bozucu ise sunulan tüm kartlar da öyle olur.",
                    ["ru"] = "«Надоеды в начале» действует и на шкуры. При каждом обмене, если хоть одна шкура с символом Надоеды, все предложенные карты получат его.",
                    ["ja"] = "「嫌がらせ」スターターが皮にも適用される。取引のたびに、「嫌がらせ」の皮が1枚でもあれば、提示されるカードすべてが「嫌がらせ」になる。",
                    ["ko"] = "구타유발자 시작이 가죽에도 적용된다. 거래할 때마다 구타유발자 가죽이 하나라도 있으면 제시되는 모든 카드가 구타유발자가 된다.",
                    ["zhcn"] = "惹人生厌的开局也作用于皮草。每次交易时，只要有一张皮草带有惹人生厌印记，提供的所有卡牌也都会带有该印记。",
                    ["zhtw"] = "煩躁起手也會影響皮草。每次交易時，只要有一張皮草帶有煩躁印記，提供的所有卡牌也都會有煩躁印記。",
                },
            };
    }
}
