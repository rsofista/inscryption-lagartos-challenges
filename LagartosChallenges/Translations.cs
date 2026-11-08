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

        // <generated-constants> (tools/new_texts.py writes this region; edit the texts there)
        /// <summary>No First Turn Kill challenge title. Also part of the challenge id, so never change it.</summary>
        public const string NoFirstTurnKillTitle = "No First Turn Kill";

        /// <summary>No First Turn Kill challenge description.</summary>
        public const string NoFirstTurnKillDescription = "Nobody can win or lose a battle on their first turn: that damage stops one short.";

        /// <summary>No Phase Turn Kill challenge title. Also part of the challenge id, so never change it.</summary>
        public const string NoPhaseTurnKillTitle = "No Phase Turn Kill";

        /// <summary>No Phase Turn Kill challenge description.</summary>
        public const string NoPhaseTurnKillDescription = "Nobody can win or lose a boss phase on their first turn after the scales reset.";

        /// <summary>Canine Only challenge title. Also part of the challenge id, so never change it.</summary>
        public const string CanineOnlyTitle = "Canine Only";

        /// <summary>Canine Only challenge description.</summary>
        public const string CanineOnlyDescription = "Only Canine cards are offered to you.";

        /// <summary>Avian Only challenge title. Also part of the challenge id, so never change it.</summary>
        public const string AvianOnlyTitle = "Avian Only";

        /// <summary>Avian Only challenge description.</summary>
        public const string AvianOnlyDescription = "Only Avian cards are offered to you.";

        /// <summary>Insect Only challenge title. Also part of the challenge id, so never change it.</summary>
        public const string InsectOnlyTitle = "Insect Only";

        /// <summary>Insect Only challenge description.</summary>
        public const string InsectOnlyDescription = "Only Insect cards are offered to you.";

        /// <summary>Reptile Only challenge title. Also part of the challenge id, so never change it.</summary>
        public const string ReptileOnlyTitle = "Reptile Only";

        /// <summary>Reptile Only challenge description.</summary>
        public const string ReptileOnlyDescription = "Only Reptile cards are offered to you.";

        /// <summary>Hooved Only challenge title. Also part of the challenge id, so never change it.</summary>
        public const string HoovedOnlyTitle = "Hooved Only";

        /// <summary>Hooved Only challenge description.</summary>
        public const string HoovedOnlyDescription = "Only Hooved cards are offered to you.";

        /// <summary>No Tribe Only challenge title. Also part of the challenge id, so never change it.</summary>
        public const string NoTribeOnlyTitle = "No Tribe Only";

        /// <summary>No Tribe Only challenge description.</summary>
        public const string NoTribeOnlyDescription = "Only cards without a tribe are offered to you.";

        /// <summary>1 Blood Only challenge title. Also part of the challenge id, so never change it.</summary>
        public const string Blood1OnlyTitle = "1 Blood Only";

        /// <summary>1 Blood Only challenge description.</summary>
        public const string Blood1OnlyDescription = "Only cards costing 1 Blood are offered to you.";

        /// <summary>2 Blood Only challenge title. Also part of the challenge id, so never change it.</summary>
        public const string Blood2OnlyTitle = "2 Blood Only";

        /// <summary>2 Blood Only challenge description.</summary>
        public const string Blood2OnlyDescription = "Only cards costing 2 Blood are offered to you.";

        /// <summary>3 Blood Only challenge title. Also part of the challenge id, so never change it.</summary>
        public const string Blood3OnlyTitle = "3 Blood Only";

        /// <summary>3 Blood Only challenge description.</summary>
        public const string Blood3OnlyDescription = "Only cards costing 3 Blood are offered to you.";

        /// <summary>4 Blood Only challenge title. Also part of the challenge id, so never change it.</summary>
        public const string Blood4OnlyTitle = "4 Blood Only";

        /// <summary>4 Blood Only challenge description.</summary>
        public const string Blood4OnlyDescription = "Only cards costing 4 Blood are offered to you.";

        /// <summary>Bones Only challenge title. Also part of the challenge id, so never change it.</summary>
        public const string BonesOnlyTitle = "Bones Only";

        /// <summary>Bones Only challenge description.</summary>
        public const string BonesOnlyDescription = "Only cards costing Bones are offered to you.";

        /// <summary>Free Only challenge title. Also part of the challenge id, so never change it.</summary>
        public const string FreeOnlyTitle = "Free Only";

        /// <summary>Free Only challenge description.</summary>
        public const string FreeOnlyDescription = "Only free cards are offered to you.";

        /// <summary>One Squirrel challenge title. Also part of the challenge id, so never change it.</summary>
        public const string OneSquirrelTitle = "One Squirrel";

        /// <summary>One Squirrel challenge description.</summary>
        public const string OneSquirrelDescription = "After your opening hand, only one Squirrel is left to draw.";

        /// <summary>Two Squirrels challenge title. Also part of the challenge id, so never change it.</summary>
        public const string TwoSquirrelsTitle = "Two Squirrels";

        /// <summary>Two Squirrels challenge description.</summary>
        public const string TwoSquirrelsDescription = "Your opening hand has a second Squirrel.";

        /// <summary>No Bone Altar challenge title. Also part of the challenge id, so never change it.</summary>
        public const string NoBoneAltarTitle = "No Bone Altar";

        /// <summary>No Bone Altar challenge description.</summary>
        public const string NoBoneAltarDescription = "The bone altar does nothing.";

        /// <summary>No Mysterious Stones challenge title. Also part of the challenge id, so never change it.</summary>
        public const string NoMysteriousStonesTitle = "No Mysterious Stones";

        /// <summary>No Mysterious Stones challenge description.</summary>
        public const string NoMysteriousStonesDescription = "The mysterious stones do nothing.";

        /// <summary>No Mycologists challenge title. Also part of the challenge id, so never change it.</summary>
        public const string NoMycologistsTitle = "No Mycologists";

        /// <summary>No Mycologists challenge description.</summary>
        public const string NoMycologistsDescription = "The Mycologists do nothing.";

        /// <summary>No Deck Trial challenge title. Also part of the challenge id, so never change it.</summary>
        public const string NoDeckTrialTitle = "No Deck Trial";

        /// <summary>No Deck Trial challenge description.</summary>
        public const string NoDeckTrialDescription = "Deck trials do nothing.";

        /// <summary>No Consumables challenge title. Also part of the challenge id, so never change it.</summary>
        public const string NoConsumablesTitle = "No Consumables";

        /// <summary>No Consumables challenge description.</summary>
        public const string NoConsumablesDescription = "Item packs give you nothing.";

        /// <summary>No Campfire challenge title. Also part of the challenge id, so never change it.</summary>
        public const string NoCampfireTitle = "No Campfire";

        /// <summary>No Campfire challenge description.</summary>
        public const string NoCampfireDescription = "Campfires do nothing.";

        /// <summary>No Buy Pelts challenge title. Also part of the challenge id, so never change it.</summary>
        public const string NoBuyPeltsTitle = "No Buy Pelts";

        /// <summary>No Buy Pelts challenge description.</summary>
        public const string NoBuyPeltsDescription = "The Trapper does nothing.";

        /// <summary>No Trade Pelts challenge title. Also part of the challenge id, so never change it.</summary>
        public const string NoTradePeltsTitle = "No Trade Pelts";

        /// <summary>No Trade Pelts challenge description.</summary>
        public const string NoTradePeltsDescription = "The Trader does nothing.";

        /// <summary>Totem Kinship challenge title. Also part of the challenge id, so never change it.</summary>
        public const string TotemKinshipTitle = "Totem Kinship";

        /// <summary>Totem Kinship challenge description.</summary>
        public const string TotemKinshipDescription = "The last totem head offered is a tribe from your deck.";
        // </generated-constants>

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
                // <generated-entries> (tools/new_texts.py)
                [NoFirstTurnKillTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Pas de victoire au premier tour",
                    ["it"] = "Niente vittoria al primo turno",
                    ["de"] = "Kein Sieg im ersten Zug",
                    ["es"] = "Sin victoria en el primer turno",
                    ["pt"] = "Sem vitória no primeiro turno",
                    ["tr"] = "İlk Turda Zafer Yok",
                    ["ru"] = "Без победы в первый ход",
                    ["ja"] = "初手決着禁止",
                    ["ko"] = "첫 턴 결판 금지",
                    ["zhcn"] = "禁止首回合决胜",
                    ["zhtw"] = "禁止首回合決勝",
                },
                [NoFirstTurnKillDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Personne ne peut gagner ou perdre un combat à son premier tour : ces dégâts s'arrêtent à un point.",
                    ["it"] = "Nessuno può vincere o perdere una battaglia al primo turno: quel danno si ferma a un punto.",
                    ["de"] = "Niemand kann einen Kampf in seinem ersten Zug gewinnen oder verlieren: Der Schaden stoppt einen Punkt davor.",
                    ["es"] = "Nadie puede ganar ni perder un combate en su primer turno: ese daño se queda a uno.",
                    ["pt"] = "Ninguém pode vencer ou perder uma batalha no primeiro turno: esse dano para a um ponto.",
                    ["tr"] = "Kimse ilk turunda bir savaşı kazanamaz ya da kaybedemez: o hasar bir eksikte durur.",
                    ["ru"] = "Никто не может выиграть или проиграть бой в свой первый ход: урон останавливается за одно очко.",
                    ["ja"] = "最初のターンでは誰もバトルに勝つことも負けることもできない。そのダメージはあと1つで止まる。",
                    ["ko"] = "누구도 첫 턴에 전투를 이기거나 질 수 없다. 그 피해는 1 모자라게 멈춘다.",
                    ["zhcn"] = "任何一方都无法在自己的第一回合赢下或输掉战斗：伤害会停在差一点的位置。",
                    ["zhtw"] = "任何一方都無法在自己的第一回合贏下或輸掉戰鬥：傷害會停在差一點的位置。",
                },
                [NoPhaseTurnKillTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Pas de victoire au premier tour de phase",
                    ["it"] = "Niente vittoria al primo turno di fase",
                    ["de"] = "Kein Sieg im ersten Phasenzug",
                    ["es"] = "Sin victoria en el primer turno de fase",
                    ["pt"] = "Sem vitória no primeiro turno da fase",
                    ["tr"] = "Aşamanın İlk Turunda Zafer Yok",
                    ["ru"] = "Без победы в первый ход фазы",
                    ["ja"] = "フェーズ初手決着禁止",
                    ["ko"] = "페이즈 첫 턴 결판 금지",
                    ["zhcn"] = "禁止阶段首回合决胜",
                    ["zhtw"] = "禁止階段首回合決勝",
                },
                [NoPhaseTurnKillDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Personne ne peut gagner ou perdre une phase de boss à son premier tour après la remise à zéro de la balance.",
                    ["it"] = "Nessuno può vincere o perdere una fase del boss al primo turno dopo l'azzeramento della bilancia.",
                    ["de"] = "Niemand kann eine Bossphase im ersten Zug nach dem Zurücksetzen der Waage gewinnen oder verlieren.",
                    ["es"] = "Nadie puede ganar ni perder una fase de jefe en su primer turno tras reiniciarse la balanza.",
                    ["pt"] = "Ninguém pode vencer ou perder uma fase de chefe no primeiro turno depois que a balança zera.",
                    ["tr"] = "Kimse terazi sıfırlandıktan sonraki ilk turunda bir patron aşamasını kazanamaz ya da kaybedemez.",
                    ["ru"] = "Никто не может выиграть или проиграть фазу босса в свой первый ход после сброса весов.",
                    ["ja"] = "天秤がリセットされた後の最初のターンでは、誰もボスのフェーズに勝つことも負けることもできない。",
                    ["ko"] = "저울이 초기화된 뒤 첫 턴에는 누구도 보스 페이즈를 이기거나 질 수 없다.",
                    ["zhcn"] = "天平重置后的第一回合，任何一方都无法赢下或输掉首领阶段。",
                    ["zhtw"] = "天平重置後的第一回合，任何一方都無法贏下或輸掉首領階段。",
                },
                [CanineOnlyTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Canidés uniquement",
                    ["it"] = "Solo canidi",
                    ["de"] = "Nur Hunde",
                    ["es"] = "Solo caninos",
                    ["pt"] = "Só Caninos",
                    ["tr"] = "Yalnızca Köpekgiller",
                    ["ru"] = "Только псовые",
                    ["ja"] = "イヌ科のみ",
                    ["ko"] = "갯과만",
                    ["zhcn"] = "仅限犬科",
                    ["zhtw"] = "僅限犬科",
                },
                [CanineOnlyDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "On ne vous propose que des cartes Canidé.",
                    ["it"] = "Ti vengono offerte solo carte Canide.",
                    ["de"] = "Dir werden nur Hunde-Karten angeboten.",
                    ["es"] = "Solo se te ofrecen cartas Canino.",
                    ["pt"] = "Só lhe oferecem cartas Canino.",
                    ["tr"] = "Sana yalnızca Köpekgil kartları sunulur.",
                    ["ru"] = "Вам предлагают только карты псовых.",
                    ["ja"] = "提示されるのはイヌ科のカードだけ。",
                    ["ko"] = "갯과 카드만 제시된다.",
                    ["zhcn"] = "只会向你提供犬科卡牌。",
                    ["zhtw"] = "只會向你提供犬科卡牌。",
                },
                [AvianOnlyTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Oiseaux uniquement",
                    ["it"] = "Solo uccelli",
                    ["de"] = "Nur Vögel",
                    ["es"] = "Solo aves",
                    ["pt"] = "Só Aves",
                    ["tr"] = "Yalnızca Kuşlar",
                    ["ru"] = "Только птицы",
                    ["ja"] = "鳥類のみ",
                    ["ko"] = "조류만",
                    ["zhcn"] = "仅限鸟类",
                    ["zhtw"] = "僅限鳥類",
                },
                [AvianOnlyDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "On ne vous propose que des cartes Oiseau.",
                    ["it"] = "Ti vengono offerte solo carte Uccello.",
                    ["de"] = "Dir werden nur Vogel-Karten angeboten.",
                    ["es"] = "Solo se te ofrecen cartas Ave.",
                    ["pt"] = "Só lhe oferecem cartas Ave.",
                    ["tr"] = "Sana yalnızca Kuş kartları sunulur.",
                    ["ru"] = "Вам предлагают только карты птиц.",
                    ["ja"] = "提示されるのは鳥類のカードだけ。",
                    ["ko"] = "조류 카드만 제시된다.",
                    ["zhcn"] = "只会向你提供鸟类卡牌。",
                    ["zhtw"] = "只會向你提供鳥類卡牌。",
                },
                [InsectOnlyTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Insectes uniquement",
                    ["it"] = "Solo insetti",
                    ["de"] = "Nur Insekten",
                    ["es"] = "Solo insectos",
                    ["pt"] = "Só Insetos",
                    ["tr"] = "Yalnızca Böcekler",
                    ["ru"] = "Только насекомые",
                    ["ja"] = "昆虫のみ",
                    ["ko"] = "곤충만",
                    ["zhcn"] = "仅限昆虫",
                    ["zhtw"] = "僅限昆蟲",
                },
                [InsectOnlyDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "On ne vous propose que des cartes Insecte.",
                    ["it"] = "Ti vengono offerte solo carte Insetto.",
                    ["de"] = "Dir werden nur Insekten-Karten angeboten.",
                    ["es"] = "Solo se te ofrecen cartas Insecto.",
                    ["pt"] = "Só lhe oferecem cartas Inseto.",
                    ["tr"] = "Sana yalnızca Böcek kartları sunulur.",
                    ["ru"] = "Вам предлагают только карты насекомых.",
                    ["ja"] = "提示されるのは昆虫のカードだけ。",
                    ["ko"] = "곤충 카드만 제시된다.",
                    ["zhcn"] = "只会向你提供昆虫卡牌。",
                    ["zhtw"] = "只會向你提供昆蟲卡牌。",
                },
                [ReptileOnlyTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Reptiles uniquement",
                    ["it"] = "Solo rettili",
                    ["de"] = "Nur Reptilien",
                    ["es"] = "Solo reptiles",
                    ["pt"] = "Só Répteis",
                    ["tr"] = "Yalnızca Sürüngenler",
                    ["ru"] = "Только рептилии",
                    ["ja"] = "爬虫類のみ",
                    ["ko"] = "파충류만",
                    ["zhcn"] = "仅限爬行类",
                    ["zhtw"] = "僅限爬行類",
                },
                [ReptileOnlyDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "On ne vous propose que des cartes Reptile.",
                    ["it"] = "Ti vengono offerte solo carte Rettile.",
                    ["de"] = "Dir werden nur Reptilien-Karten angeboten.",
                    ["es"] = "Solo se te ofrecen cartas Reptil.",
                    ["pt"] = "Só lhe oferecem cartas Réptil.",
                    ["tr"] = "Sana yalnızca Sürüngen kartları sunulur.",
                    ["ru"] = "Вам предлагают только карты рептилий.",
                    ["ja"] = "提示されるのは爬虫類のカードだけ。",
                    ["ko"] = "파충류 카드만 제시된다.",
                    ["zhcn"] = "只会向你提供爬行类卡牌。",
                    ["zhtw"] = "只會向你提供爬行類卡牌。",
                },
                [HoovedOnlyTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Ongulés uniquement",
                    ["it"] = "Solo ungulati",
                    ["de"] = "Nur Huftiere",
                    ["es"] = "Solo ungulados",
                    ["pt"] = "Só Cascos",
                    ["tr"] = "Yalnızca Toynaklılar",
                    ["ru"] = "Только копытные",
                    ["ja"] = "有蹄類のみ",
                    ["ko"] = "발굽 동물만",
                    ["zhcn"] = "仅限有蹄类",
                    ["zhtw"] = "僅限有蹄類",
                },
                [HoovedOnlyDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "On ne vous propose que des cartes Ongulé.",
                    ["it"] = "Ti vengono offerte solo carte Ungulato.",
                    ["de"] = "Dir werden nur Huftier-Karten angeboten.",
                    ["es"] = "Solo se te ofrecen cartas Ungulado.",
                    ["pt"] = "Só lhe oferecem cartas com Cascos.",
                    ["tr"] = "Sana yalnızca Toynaklı kartları sunulur.",
                    ["ru"] = "Вам предлагают только карты копытных.",
                    ["ja"] = "提示されるのは有蹄類のカードだけ。",
                    ["ko"] = "발굽 동물 카드만 제시된다.",
                    ["zhcn"] = "只会向你提供有蹄类卡牌。",
                    ["zhtw"] = "只會向你提供有蹄類卡牌。",
                },
                [NoTribeOnlyTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Sans tribu uniquement",
                    ["it"] = "Solo senza tribù",
                    ["de"] = "Nur ohne Stamm",
                    ["es"] = "Solo sin tribu",
                    ["pt"] = "Só Sem Tribo",
                    ["tr"] = "Yalnızca Kabilesizler",
                    ["ru"] = "Только без племени",
                    ["ja"] = "種族なしのみ",
                    ["ko"] = "부족 없음만",
                    ["zhcn"] = "仅限无族群",
                    ["zhtw"] = "僅限無族群",
                },
                [NoTribeOnlyDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "On ne vous propose que des cartes sans tribu.",
                    ["it"] = "Ti vengono offerte solo carte senza tribù.",
                    ["de"] = "Dir werden nur Karten ohne Stamm angeboten.",
                    ["es"] = "Solo se te ofrecen cartas sin tribu.",
                    ["pt"] = "Só lhe oferecem cartas sem tribo.",
                    ["tr"] = "Sana yalnızca kabilesi olmayan kartlar sunulur.",
                    ["ru"] = "Вам предлагают только карты без племени.",
                    ["ja"] = "提示されるのは種族のないカードだけ。",
                    ["ko"] = "부족이 없는 카드만 제시된다.",
                    ["zhcn"] = "只会向你提供没有族群的卡牌。",
                    ["zhtw"] = "只會向你提供沒有族群的卡牌。",
                },
                [Blood1OnlyTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "1 sang uniquement",
                    ["it"] = "Solo 1 sangue",
                    ["de"] = "Nur 1 Blut",
                    ["es"] = "Solo 1 de sangre",
                    ["pt"] = "Só 1 de Sangue",
                    ["tr"] = "Yalnızca 1 Kan",
                    ["ru"] = "Только 1 крови",
                    ["ja"] = "血1のみ",
                    ["ko"] = "피 1만",
                    ["zhcn"] = "仅限1血",
                    ["zhtw"] = "僅限1血",
                },
                [Blood1OnlyDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "On ne vous propose que des cartes coûtant 1 sang.",
                    ["it"] = "Ti vengono offerte solo carte che costano 1 sangue.",
                    ["de"] = "Dir werden nur Karten angeboten, die 1 Blut kosten.",
                    ["es"] = "Solo se te ofrecen cartas que cuestan 1 de sangre.",
                    ["pt"] = "Só lhe oferecem cartas que custam 1 de sangue.",
                    ["tr"] = "Sana yalnızca 1 Kan değerindeki kartlar sunulur.",
                    ["ru"] = "Вам предлагают только карты стоимостью 1 крови.",
                    ["ja"] = "提示されるのはコストが血1のカードだけ。",
                    ["ko"] = "피 1 비용의 카드만 제시된다.",
                    ["zhcn"] = "只会向你提供消耗1点鲜血的卡牌。",
                    ["zhtw"] = "只會向你提供消耗1點鮮血的卡牌。",
                },
                [Blood2OnlyTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "2 sang uniquement",
                    ["it"] = "Solo 2 sangue",
                    ["de"] = "Nur 2 Blut",
                    ["es"] = "Solo 2 de sangre",
                    ["pt"] = "Só 2 de Sangue",
                    ["tr"] = "Yalnızca 2 Kan",
                    ["ru"] = "Только 2 крови",
                    ["ja"] = "血2のみ",
                    ["ko"] = "피 2만",
                    ["zhcn"] = "仅限2血",
                    ["zhtw"] = "僅限2血",
                },
                [Blood2OnlyDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "On ne vous propose que des cartes coûtant 2 sang.",
                    ["it"] = "Ti vengono offerte solo carte che costano 2 sangue.",
                    ["de"] = "Dir werden nur Karten angeboten, die 2 Blut kosten.",
                    ["es"] = "Solo se te ofrecen cartas que cuestan 2 de sangre.",
                    ["pt"] = "Só lhe oferecem cartas que custam 2 de sangue.",
                    ["tr"] = "Sana yalnızca 2 Kan değerindeki kartlar sunulur.",
                    ["ru"] = "Вам предлагают только карты стоимостью 2 крови.",
                    ["ja"] = "提示されるのはコストが血2のカードだけ。",
                    ["ko"] = "피 2 비용의 카드만 제시된다.",
                    ["zhcn"] = "只会向你提供消耗2点鲜血的卡牌。",
                    ["zhtw"] = "只會向你提供消耗2點鮮血的卡牌。",
                },
                [Blood3OnlyTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "3 sang uniquement",
                    ["it"] = "Solo 3 sangue",
                    ["de"] = "Nur 3 Blut",
                    ["es"] = "Solo 3 de sangre",
                    ["pt"] = "Só 3 de Sangue",
                    ["tr"] = "Yalnızca 3 Kan",
                    ["ru"] = "Только 3 крови",
                    ["ja"] = "血3のみ",
                    ["ko"] = "피 3만",
                    ["zhcn"] = "仅限3血",
                    ["zhtw"] = "僅限3血",
                },
                [Blood3OnlyDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "On ne vous propose que des cartes coûtant 3 sang.",
                    ["it"] = "Ti vengono offerte solo carte che costano 3 sangue.",
                    ["de"] = "Dir werden nur Karten angeboten, die 3 Blut kosten.",
                    ["es"] = "Solo se te ofrecen cartas que cuestan 3 de sangre.",
                    ["pt"] = "Só lhe oferecem cartas que custam 3 de sangue.",
                    ["tr"] = "Sana yalnızca 3 Kan değerindeki kartlar sunulur.",
                    ["ru"] = "Вам предлагают только карты стоимостью 3 крови.",
                    ["ja"] = "提示されるのはコストが血3のカードだけ。",
                    ["ko"] = "피 3 비용의 카드만 제시된다.",
                    ["zhcn"] = "只会向你提供消耗3点鲜血的卡牌。",
                    ["zhtw"] = "只會向你提供消耗3點鮮血的卡牌。",
                },
                [Blood4OnlyTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "4 sang uniquement",
                    ["it"] = "Solo 4 sangue",
                    ["de"] = "Nur 4 Blut",
                    ["es"] = "Solo 4 de sangre",
                    ["pt"] = "Só 4 de Sangue",
                    ["tr"] = "Yalnızca 4 Kan",
                    ["ru"] = "Только 4 крови",
                    ["ja"] = "血4のみ",
                    ["ko"] = "피 4만",
                    ["zhcn"] = "仅限4血",
                    ["zhtw"] = "僅限4血",
                },
                [Blood4OnlyDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "On ne vous propose que des cartes coûtant 4 sang.",
                    ["it"] = "Ti vengono offerte solo carte che costano 4 sangue.",
                    ["de"] = "Dir werden nur Karten angeboten, die 4 Blut kosten.",
                    ["es"] = "Solo se te ofrecen cartas que cuestan 4 de sangre.",
                    ["pt"] = "Só lhe oferecem cartas que custam 4 de sangue.",
                    ["tr"] = "Sana yalnızca 4 Kan değerindeki kartlar sunulur.",
                    ["ru"] = "Вам предлагают только карты стоимостью 4 крови.",
                    ["ja"] = "提示されるのはコストが血4のカードだけ。",
                    ["ko"] = "피 4 비용의 카드만 제시된다.",
                    ["zhcn"] = "只会向你提供消耗4点鲜血的卡牌。",
                    ["zhtw"] = "只會向你提供消耗4點鮮血的卡牌。",
                },
                [BonesOnlyTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Os uniquement",
                    ["it"] = "Solo ossa",
                    ["de"] = "Nur Knochen",
                    ["es"] = "Solo huesos",
                    ["pt"] = "Só Ossos",
                    ["tr"] = "Yalnızca Kemik",
                    ["ru"] = "Только кости",
                    ["ja"] = "骨のみ",
                    ["ko"] = "뼈만",
                    ["zhcn"] = "仅限骨头",
                    ["zhtw"] = "僅限骨頭",
                },
                [BonesOnlyDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "On ne vous propose que des cartes coûtant des os.",
                    ["it"] = "Ti vengono offerte solo carte che costano ossa.",
                    ["de"] = "Dir werden nur Karten angeboten, die Knochen kosten.",
                    ["es"] = "Solo se te ofrecen cartas que cuestan huesos.",
                    ["pt"] = "Só lhe oferecem cartas que custam ossos.",
                    ["tr"] = "Sana yalnızca Kemik değerindeki kartlar sunulur.",
                    ["ru"] = "Вам предлагают только карты, стоящие костей.",
                    ["ja"] = "提示されるのはコストが骨のカードだけ。",
                    ["ko"] = "뼈 비용의 카드만 제시된다.",
                    ["zhcn"] = "只会向你提供消耗骨头的卡牌。",
                    ["zhtw"] = "只會向你提供消耗骨頭的卡牌。",
                },
                [FreeOnlyTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Gratuites uniquement",
                    ["it"] = "Solo gratuite",
                    ["de"] = "Nur kostenlos",
                    ["es"] = "Solo gratis",
                    ["pt"] = "Só Grátis",
                    ["tr"] = "Yalnızca Bedava",
                    ["ru"] = "Только бесплатные",
                    ["ja"] = "コストなしのみ",
                    ["ko"] = "무료만",
                    ["zhcn"] = "仅限免费",
                    ["zhtw"] = "僅限免費",
                },
                [FreeOnlyDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "On ne vous propose que des cartes gratuites.",
                    ["it"] = "Ti vengono offerte solo carte gratuite.",
                    ["de"] = "Dir werden nur kostenlose Karten angeboten.",
                    ["es"] = "Solo se te ofrecen cartas gratuitas.",
                    ["pt"] = "Só lhe oferecem cartas gratuitas.",
                    ["tr"] = "Sana yalnızca bedava kartlar sunulur.",
                    ["ru"] = "Вам предлагают только бесплатные карты.",
                    ["ja"] = "提示されるのはコストのないカードだけ。",
                    ["ko"] = "비용이 없는 카드만 제시된다.",
                    ["zhcn"] = "只会向你提供无消耗的卡牌。",
                    ["zhtw"] = "只會向你提供無消耗的卡牌。",
                },
                [OneSquirrelTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Un seul écureuil",
                    ["it"] = "Un solo scoiattolo",
                    ["de"] = "Ein Eichhörnchen",
                    ["es"] = "Una ardilla",
                    ["pt"] = "Um Esquilo",
                    ["tr"] = "Tek Sincap",
                    ["ru"] = "Одна белка",
                    ["ja"] = "リス1匹",
                    ["ko"] = "다람쥐 한 마리",
                    ["zhcn"] = "一只松鼠",
                    ["zhtw"] = "一隻松鼠",
                },
                [OneSquirrelDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Après votre main de départ, il ne reste qu'un écureuil à piocher.",
                    ["it"] = "Dopo la mano iniziale resta un solo scoiattolo da pescare.",
                    ["de"] = "Nach deiner Starthand bleibt nur ein Eichhörnchen zum Ziehen übrig.",
                    ["es"] = "Tras tu mano inicial, solo queda una ardilla por robar.",
                    ["pt"] = "Depois da mão inicial, só resta um esquilo para comprar.",
                    ["tr"] = "Başlangıç elinden sonra çekilecek tek bir sincap kalır.",
                    ["ru"] = "После стартовой руки в колоде остаётся только одна белка.",
                    ["ja"] = "最初の手札の後、引けるリスは1匹しか残らない。",
                    ["ko"] = "시작 패 이후 뽑을 다람쥐가 한 마리만 남는다.",
                    ["zhcn"] = "起手之后，只剩一只松鼠可以抽。",
                    ["zhtw"] = "起手之後，只剩一隻松鼠可以抽。",
                },
                [TwoSquirrelsTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Deux écureuils",
                    ["it"] = "Due scoiattoli",
                    ["de"] = "Zwei Eichhörnchen",
                    ["es"] = "Dos ardillas",
                    ["pt"] = "Dois Esquilos",
                    ["tr"] = "İki Sincap",
                    ["ru"] = "Две белки",
                    ["ja"] = "リス2匹",
                    ["ko"] = "다람쥐 두 마리",
                    ["zhcn"] = "两只松鼠",
                    ["zhtw"] = "兩隻松鼠",
                },
                [TwoSquirrelsDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Votre main de départ contient un deuxième écureuil.",
                    ["it"] = "La tua mano iniziale ha un secondo scoiattolo.",
                    ["de"] = "Deine Starthand enthält ein zweites Eichhörnchen.",
                    ["es"] = "Tu mano inicial tiene una segunda ardilla.",
                    ["pt"] = "Sua mão inicial tem um segundo esquilo.",
                    ["tr"] = "Başlangıç elinde ikinci bir sincap olur.",
                    ["ru"] = "В стартовой руке есть вторая белка.",
                    ["ja"] = "最初の手札にリスがもう1匹加わる。",
                    ["ko"] = "시작 패에 다람쥐가 한 마리 더 있다.",
                    ["zhcn"] = "起手多一只松鼠。",
                    ["zhtw"] = "起手多一隻松鼠。",
                },
                [NoBoneAltarTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Pas d'autel d'os",
                    ["it"] = "Niente altare d'ossa",
                    ["de"] = "Kein Knochenaltar",
                    ["es"] = "Sin altar de huesos",
                    ["pt"] = "Sem Altar de Ossos",
                    ["tr"] = "Kemik Sunağı Yok",
                    ["ru"] = "Без костяного алтаря",
                    ["ja"] = "骨の祭壇なし",
                    ["ko"] = "뼈 제단 없음",
                    ["zhcn"] = "没有骨祭坛",
                    ["zhtw"] = "沒有骨祭壇",
                },
                [NoBoneAltarDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "L'autel d'os ne fait rien.",
                    ["it"] = "L'altare d'ossa non fa nulla.",
                    ["de"] = "Der Knochenaltar bewirkt nichts.",
                    ["es"] = "El altar de huesos no hace nada.",
                    ["pt"] = "O altar de ossos não faz nada.",
                    ["tr"] = "Kemik sunağı hiçbir şey yapmaz.",
                    ["ru"] = "Костяной алтарь ничего не делает.",
                    ["ja"] = "骨の祭壇は何もしない。",
                    ["ko"] = "뼈 제단은 아무것도 하지 않는다.",
                    ["zhcn"] = "骨祭坛毫无作用。",
                    ["zhtw"] = "骨祭壇毫無作用。",
                },
                [NoMysteriousStonesTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Pas de pierres mystérieuses",
                    ["it"] = "Niente pietre misteriose",
                    ["de"] = "Keine mysteriösen Steine",
                    ["es"] = "Sin piedras misteriosas",
                    ["pt"] = "Sem Pedras Misteriosas",
                    ["tr"] = "Gizemli Taşlar Yok",
                    ["ru"] = "Без таинственных камней",
                    ["ja"] = "謎の石なし",
                    ["ko"] = "신비한 돌 없음",
                    ["zhcn"] = "没有神秘石头",
                    ["zhtw"] = "沒有神秘石頭",
                },
                [NoMysteriousStonesDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Les pierres mystérieuses ne font rien.",
                    ["it"] = "Le pietre misteriose non fanno nulla.",
                    ["de"] = "Die mysteriösen Steine bewirken nichts.",
                    ["es"] = "Las piedras misteriosas no hacen nada.",
                    ["pt"] = "As pedras misteriosas não fazem nada.",
                    ["tr"] = "Gizemli taşlar hiçbir şey yapmaz.",
                    ["ru"] = "Таинственные камни ничего не делают.",
                    ["ja"] = "謎の石は何もしない。",
                    ["ko"] = "신비한 돌은 아무것도 하지 않는다.",
                    ["zhcn"] = "神秘石头毫无作用。",
                    ["zhtw"] = "神秘石頭毫無作用。",
                },
                [NoMycologistsTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Pas de mycologues",
                    ["it"] = "Niente micologi",
                    ["de"] = "Keine Mykologen",
                    ["es"] = "Sin micólogos",
                    ["pt"] = "Sem Micologistas",
                    ["tr"] = "Mantarbilimciler Yok",
                    ["ru"] = "Без микологов",
                    ["ja"] = "菌類学者なし",
                    ["ko"] = "균류학자 없음",
                    ["zhcn"] = "没有真菌学家",
                    ["zhtw"] = "沒有真菌學家",
                },
                [NoMycologistsDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Les mycologues ne font rien.",
                    ["it"] = "I micologi non fanno nulla.",
                    ["de"] = "Die Mykologen bewirken nichts.",
                    ["es"] = "Los micólogos no hacen nada.",
                    ["pt"] = "Os micologistas não fazem nada.",
                    ["tr"] = "Mantarbilimciler hiçbir şey yapmaz.",
                    ["ru"] = "Микологи ничего не делают.",
                    ["ja"] = "菌類学者は何もしない。",
                    ["ko"] = "균류학자는 아무것도 하지 않는다.",
                    ["zhcn"] = "真菌学家毫无作用。",
                    ["zhtw"] = "真菌學家毫無作用。",
                },
                [NoDeckTrialTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Pas d'épreuve de deck",
                    ["it"] = "Niente prove del mazzo",
                    ["de"] = "Keine Deckprüfung",
                    ["es"] = "Sin pruebas de mazo",
                    ["pt"] = "Sem Prova do Baralho",
                    ["tr"] = "Deste Sınavı Yok",
                    ["ru"] = "Без испытаний колоды",
                    ["ja"] = "デッキの試練なし",
                    ["ko"] = "덱 시험 없음",
                    ["zhcn"] = "没有牌组试炼",
                    ["zhtw"] = "沒有牌組試煉",
                },
                [NoDeckTrialDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Les épreuves de deck ne font rien.",
                    ["it"] = "Le prove del mazzo non fanno nulla.",
                    ["de"] = "Deckprüfungen bewirken nichts.",
                    ["es"] = "Las pruebas de mazo no hacen nada.",
                    ["pt"] = "As provas do baralho não fazem nada.",
                    ["tr"] = "Deste sınavları hiçbir şey yapmaz.",
                    ["ru"] = "Испытания колоды ничего не делают.",
                    ["ja"] = "デッキの試練は何もしない。",
                    ["ko"] = "덱 시험은 아무것도 하지 않는다.",
                    ["zhcn"] = "牌组试炼毫无作用。",
                    ["zhtw"] = "牌組試煉毫無作用。",
                },
                [NoConsumablesTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Pas de consommables",
                    ["it"] = "Niente consumabili",
                    ["de"] = "Keine Verbrauchsgüter",
                    ["es"] = "Sin consumibles",
                    ["pt"] = "Sem Consumíveis",
                    ["tr"] = "Sarf Malzemesi Yok",
                    ["ru"] = "Без расходников",
                    ["ja"] = "消耗品なし",
                    ["ko"] = "소모품 없음",
                    ["zhcn"] = "没有消耗品",
                    ["zhtw"] = "沒有消耗品",
                },
                [NoConsumablesDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Les sacs d'objets ne vous donnent rien.",
                    ["it"] = "Gli zaini di oggetti non ti danno nulla.",
                    ["de"] = "Gegenstandsbeutel geben dir nichts.",
                    ["es"] = "Las mochilas de objetos no te dan nada.",
                    ["pt"] = "As mochilas de itens não lhe dão nada.",
                    ["tr"] = "Eşya çantaları sana hiçbir şey vermez.",
                    ["ru"] = "Сумки с предметами ничего не дают.",
                    ["ja"] = "アイテムの袋からは何も得られない。",
                    ["ko"] = "아이템 꾸러미는 아무것도 주지 않는다.",
                    ["zhcn"] = "物品背包什么也不给你。",
                    ["zhtw"] = "物品背包什麼也不給你。",
                },
                [NoCampfireTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Pas de feu de camp",
                    ["it"] = "Niente falò",
                    ["de"] = "Kein Lagerfeuer",
                    ["es"] = "Sin hogueras",
                    ["pt"] = "Sem Fogueira",
                    ["tr"] = "Kamp Ateşi Yok",
                    ["ru"] = "Без костра",
                    ["ja"] = "焚き火なし",
                    ["ko"] = "모닥불 없음",
                    ["zhcn"] = "没有篝火",
                    ["zhtw"] = "沒有篝火",
                },
                [NoCampfireDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Les feux de camp ne font rien.",
                    ["it"] = "I falò non fanno nulla.",
                    ["de"] = "Lagerfeuer bewirken nichts.",
                    ["es"] = "Las hogueras no hacen nada.",
                    ["pt"] = "As fogueiras não fazem nada.",
                    ["tr"] = "Kamp ateşleri hiçbir şey yapmaz.",
                    ["ru"] = "Костры ничего не делают.",
                    ["ja"] = "焚き火は何もしない。",
                    ["ko"] = "모닥불은 아무것도 하지 않는다.",
                    ["zhcn"] = "篝火毫无作用。",
                    ["zhtw"] = "篝火毫無作用。",
                },
                [NoBuyPeltsTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Pas d'achat de fourrures",
                    ["it"] = "Niente acquisto di pelli",
                    ["de"] = "Kein Fellkauf",
                    ["es"] = "Sin comprar pieles",
                    ["pt"] = "Sem Comprar Peles",
                    ["tr"] = "Post Satın Alma Yok",
                    ["ru"] = "Без покупки шкур",
                    ["ja"] = "皮の購入なし",
                    ["ko"] = "가죽 구매 없음",
                    ["zhcn"] = "没有购买皮草",
                    ["zhtw"] = "沒有購買皮草",
                },
                [NoBuyPeltsDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Le Trappeur ne fait rien.",
                    ["it"] = "Il Cacciatore non fa nulla.",
                    ["de"] = "Der Fallensteller bewirkt nichts.",
                    ["es"] = "El Trampero no hace nada.",
                    ["pt"] = "O Caçador não faz nada.",
                    ["tr"] = "Tuzakçı hiçbir şey yapmaz.",
                    ["ru"] = "Охотник ничего не делает.",
                    ["ja"] = "罠師は何もしない。",
                    ["ko"] = "덫사냥꾼은 아무것도 하지 않는다.",
                    ["zhcn"] = "猎人毫无作用。",
                    ["zhtw"] = "獵人毫無作用。",
                },
                [NoTradePeltsTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Pas d'échange de fourrures",
                    ["it"] = "Niente scambio di pelli",
                    ["de"] = "Kein Felltausch",
                    ["es"] = "Sin trueque de pieles",
                    ["pt"] = "Sem Trocar Peles",
                    ["tr"] = "Post Takası Yok",
                    ["ru"] = "Без обмена шкур",
                    ["ja"] = "皮の取引なし",
                    ["ko"] = "가죽 거래 없음",
                    ["zhcn"] = "没有交易皮草",
                    ["zhtw"] = "沒有交易皮草",
                },
                [NoTradePeltsDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "Le Marchand ne fait rien.",
                    ["it"] = "Il Mercante non fa nulla.",
                    ["de"] = "Der Händler bewirkt nichts.",
                    ["es"] = "El Comerciante no hace nada.",
                    ["pt"] = "O Comerciante não faz nada.",
                    ["tr"] = "Tüccar hiçbir şey yapmaz.",
                    ["ru"] = "Торговец ничего не делает.",
                    ["ja"] = "商人は何もしない。",
                    ["ko"] = "상인은 아무것도 하지 않는다.",
                    ["zhcn"] = "商人毫无作用。",
                    ["zhtw"] = "商人毫無作用。",
                },
                [TotemKinshipTitle] = new Dictionary<string, string>
                {
                    ["fr"] = "Totem de parenté",
                    ["it"] = "Totem affine",
                    ["de"] = "Totem-Verwandtschaft",
                    ["es"] = "Tótem afín",
                    ["pt"] = "Totem Afim",
                    ["tr"] = "Totem Akrabalığı",
                    ["ru"] = "Родство тотема",
                    ["ja"] = "トーテムの絆",
                    ["ko"] = "토템 친족",
                    ["zhcn"] = "图腾亲缘",
                    ["zhtw"] = "圖騰親緣",
                },
                [TotemKinshipDescription] = new Dictionary<string, string>
                {
                    ["fr"] = "La dernière tête de totem proposée est une tribu de votre deck.",
                    ["it"] = "L'ultima testa di totem offerta è una tribù del tuo mazzo.",
                    ["de"] = "Der letzte angebotene Totemkopf ist ein Stamm aus deinem Deck.",
                    ["es"] = "La última cabeza de tótem ofrecida es una tribu de tu mazo.",
                    ["pt"] = "A última cabeça de totem oferecida é uma tribo do seu baralho.",
                    ["tr"] = "Sunulan son totem başı destendeki bir kabiledir.",
                    ["ru"] = "Последняя предложенная голова тотема — племя из вашей колоды.",
                    ["ja"] = "最後に提示されるトーテムの頭は、デッキにある種族になる。",
                    ["ko"] = "마지막으로 제시되는 토템 머리는 덱에 있는 부족이다.",
                    ["zhcn"] = "最后提供的图腾头会是你牌组中的族群。",
                    ["zhtw"] = "最後提供的圖騰頭會是你牌組中的族群。",
                },
                // </generated-entries>
            };
    }
}
