"""Source of the 24 new challenges' texts in the 12 game languages.

Run: python tools/new_texts.py  -> rewrites the generated region of LagartosChallenges/Translations.cs
(between the "// <generated>" and "// </generated>" markers). Edit the texts here, not in the .cs file.
"""
import os
import re

ROOT = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..")
TARGET = os.path.join(ROOT, "LagartosChallenges", "Translations.cs")
LANGS = ["fr", "it", "de", "es", "pt", "tr", "ru", "ja", "ko", "zhcn", "zhtw"]

# (C# constant prefix, english title, english description, {lang: (title, description)})
TEXTS = [
    ("NoFirstTurnKill", "No First Turn Kill",
     "Nobody can win or lose a battle on their first turn: that damage stops one short.", {
         "fr": ("Pas de victoire au premier tour", "Personne ne peut gagner ou perdre un combat à son premier tour : ces dégâts s'arrêtent à un point."),
         "it": ("Niente vittoria al primo turno", "Nessuno può vincere o perdere una battaglia al primo turno: quel danno si ferma a un punto."),
         "de": ("Kein Sieg im ersten Zug", "Niemand kann einen Kampf in seinem ersten Zug gewinnen oder verlieren: Der Schaden stoppt einen Punkt davor."),
         "es": ("Sin victoria en el primer turno", "Nadie puede ganar ni perder un combate en su primer turno: ese daño se queda a uno."),
         "pt": ("Sem vitória no primeiro turno", "Ninguém pode vencer ou perder uma batalha no primeiro turno: esse dano para a um ponto."),
         "tr": ("İlk Turda Zafer Yok", "Kimse ilk turunda bir savaşı kazanamaz ya da kaybedemez: o hasar bir eksikte durur."),
         "ru": ("Без победы в первый ход", "Никто не может выиграть или проиграть бой в свой первый ход: урон останавливается за одно очко."),
         "ja": ("初手決着禁止", "最初のターンでは誰もバトルに勝つことも負けることもできない。そのダメージはあと1つで止まる。"),
         "ko": ("첫 턴 결판 금지", "누구도 첫 턴에 전투를 이기거나 질 수 없다. 그 피해는 1 모자라게 멈춘다."),
         "zhcn": ("禁止首回合决胜", "任何一方都无法在自己的第一回合赢下或输掉战斗：伤害会停在差一点的位置。"),
         "zhtw": ("禁止首回合決勝", "任何一方都無法在自己的第一回合贏下或輸掉戰鬥：傷害會停在差一點的位置。"),
     }),
    ("NoPhaseTurnKill", "No Phase Turn Kill",
     "Nobody can win or lose a boss phase on their first turn after the scales reset.", {
         "fr": ("Pas de victoire au premier tour de phase", "Personne ne peut gagner ou perdre une phase de boss à son premier tour après la remise à zéro de la balance."),
         "it": ("Niente vittoria al primo turno di fase", "Nessuno può vincere o perdere una fase del boss al primo turno dopo l'azzeramento della bilancia."),
         "de": ("Kein Sieg im ersten Phasenzug", "Niemand kann eine Bossphase im ersten Zug nach dem Zurücksetzen der Waage gewinnen oder verlieren."),
         "es": ("Sin victoria en el primer turno de fase", "Nadie puede ganar ni perder una fase de jefe en su primer turno tras reiniciarse la balanza."),
         "pt": ("Sem vitória no primeiro turno da fase", "Ninguém pode vencer ou perder uma fase de chefe no primeiro turno depois que a balança zera."),
         "tr": ("Aşamanın İlk Turunda Zafer Yok", "Kimse terazi sıfırlandıktan sonraki ilk turunda bir patron aşamasını kazanamaz ya da kaybedemez."),
         "ru": ("Без победы в первый ход фазы", "Никто не может выиграть или проиграть фазу босса в свой первый ход после сброса весов."),
         "ja": ("フェーズ初手決着禁止", "天秤がリセットされた後の最初のターンでは、誰もボスのフェーズに勝つことも負けることもできない。"),
         "ko": ("페이즈 첫 턴 결판 금지", "저울이 초기화된 뒤 첫 턴에는 누구도 보스 페이즈를 이기거나 질 수 없다."),
         "zhcn": ("禁止阶段首回合决胜", "天平重置后的第一回合，任何一方都无法赢下或输掉首领阶段。"),
         "zhtw": ("禁止階段首回合決勝", "天平重置後的第一回合，任何一方都無法贏下或輸掉首領階段。"),
     }),
    ("CanineOnly", "Canine Only", "Only Canine cards are offered to you.", {
        "fr": ("Canidés uniquement", "On ne vous propose que des cartes Canidé."),
        "it": ("Solo canidi", "Ti vengono offerte solo carte Canide."),
        "de": ("Nur Hunde", "Dir werden nur Hunde-Karten angeboten."),
        "es": ("Solo caninos", "Solo se te ofrecen cartas Canino."),
        "pt": ("Só Caninos", "Só lhe oferecem cartas Canino."),
        "tr": ("Yalnızca Köpekgiller", "Sana yalnızca Köpekgil kartları sunulur."),
        "ru": ("Только псовые", "Вам предлагают только карты псовых."),
        "ja": ("イヌ科のみ", "提示されるのはイヌ科のカードだけ。"),
        "ko": ("갯과만", "갯과 카드만 제시된다."),
        "zhcn": ("仅限犬科", "只会向你提供犬科卡牌。"),
        "zhtw": ("僅限犬科", "只會向你提供犬科卡牌。"),
    }),
    ("AvianOnly", "Avian Only", "Only Avian cards are offered to you.", {
        "fr": ("Oiseaux uniquement", "On ne vous propose que des cartes Oiseau."),
        "it": ("Solo uccelli", "Ti vengono offerte solo carte Uccello."),
        "de": ("Nur Vögel", "Dir werden nur Vogel-Karten angeboten."),
        "es": ("Solo aves", "Solo se te ofrecen cartas Ave."),
        "pt": ("Só Aves", "Só lhe oferecem cartas Ave."),
        "tr": ("Yalnızca Kuşlar", "Sana yalnızca Kuş kartları sunulur."),
        "ru": ("Только птицы", "Вам предлагают только карты птиц."),
        "ja": ("鳥類のみ", "提示されるのは鳥類のカードだけ。"),
        "ko": ("조류만", "조류 카드만 제시된다."),
        "zhcn": ("仅限鸟类", "只会向你提供鸟类卡牌。"),
        "zhtw": ("僅限鳥類", "只會向你提供鳥類卡牌。"),
    }),
    ("InsectOnly", "Insect Only", "Only Insect cards are offered to you.", {
        "fr": ("Insectes uniquement", "On ne vous propose que des cartes Insecte."),
        "it": ("Solo insetti", "Ti vengono offerte solo carte Insetto."),
        "de": ("Nur Insekten", "Dir werden nur Insekten-Karten angeboten."),
        "es": ("Solo insectos", "Solo se te ofrecen cartas Insecto."),
        "pt": ("Só Insetos", "Só lhe oferecem cartas Inseto."),
        "tr": ("Yalnızca Böcekler", "Sana yalnızca Böcek kartları sunulur."),
        "ru": ("Только насекомые", "Вам предлагают только карты насекомых."),
        "ja": ("昆虫のみ", "提示されるのは昆虫のカードだけ。"),
        "ko": ("곤충만", "곤충 카드만 제시된다."),
        "zhcn": ("仅限昆虫", "只会向你提供昆虫卡牌。"),
        "zhtw": ("僅限昆蟲", "只會向你提供昆蟲卡牌。"),
    }),
    ("ReptileOnly", "Reptile Only", "Only Reptile cards are offered to you.", {
        "fr": ("Reptiles uniquement", "On ne vous propose que des cartes Reptile."),
        "it": ("Solo rettili", "Ti vengono offerte solo carte Rettile."),
        "de": ("Nur Reptilien", "Dir werden nur Reptilien-Karten angeboten."),
        "es": ("Solo reptiles", "Solo se te ofrecen cartas Reptil."),
        "pt": ("Só Répteis", "Só lhe oferecem cartas Réptil."),
        "tr": ("Yalnızca Sürüngenler", "Sana yalnızca Sürüngen kartları sunulur."),
        "ru": ("Только рептилии", "Вам предлагают только карты рептилий."),
        "ja": ("爬虫類のみ", "提示されるのは爬虫類のカードだけ。"),
        "ko": ("파충류만", "파충류 카드만 제시된다."),
        "zhcn": ("仅限爬行类", "只会向你提供爬行类卡牌。"),
        "zhtw": ("僅限爬行類", "只會向你提供爬行類卡牌。"),
    }),
    ("HoovedOnly", "Hooved Only", "Only Hooved cards are offered to you.", {
        "fr": ("Ongulés uniquement", "On ne vous propose que des cartes Ongulé."),
        "it": ("Solo ungulati", "Ti vengono offerte solo carte Ungulato."),
        "de": ("Nur Huftiere", "Dir werden nur Huftier-Karten angeboten."),
        "es": ("Solo ungulados", "Solo se te ofrecen cartas Ungulado."),
        "pt": ("Só Cascos", "Só lhe oferecem cartas com Cascos."),
        "tr": ("Yalnızca Toynaklılar", "Sana yalnızca Toynaklı kartları sunulur."),
        "ru": ("Только копытные", "Вам предлагают только карты копытных."),
        "ja": ("有蹄類のみ", "提示されるのは有蹄類のカードだけ。"),
        "ko": ("발굽 동물만", "발굽 동물 카드만 제시된다."),
        "zhcn": ("仅限有蹄类", "只会向你提供有蹄类卡牌。"),
        "zhtw": ("僅限有蹄類", "只會向你提供有蹄類卡牌。"),
    }),
    ("NoTribeOnly", "No Tribe Only", "Only cards without a tribe are offered to you.", {
        "fr": ("Sans tribu uniquement", "On ne vous propose que des cartes sans tribu."),
        "it": ("Solo senza tribù", "Ti vengono offerte solo carte senza tribù."),
        "de": ("Nur ohne Stamm", "Dir werden nur Karten ohne Stamm angeboten."),
        "es": ("Solo sin tribu", "Solo se te ofrecen cartas sin tribu."),
        "pt": ("Só Sem Tribo", "Só lhe oferecem cartas sem tribo."),
        "tr": ("Yalnızca Kabilesizler", "Sana yalnızca kabilesi olmayan kartlar sunulur."),
        "ru": ("Только без племени", "Вам предлагают только карты без племени."),
        "ja": ("種族なしのみ", "提示されるのは種族のないカードだけ。"),
        "ko": ("부족 없음만", "부족이 없는 카드만 제시된다."),
        "zhcn": ("仅限无族群", "只会向你提供没有族群的卡牌。"),
        "zhtw": ("僅限無族群", "只會向你提供沒有族群的卡牌。"),
    }),
]


def blood(n):
    """Entry for the 'N Blood Only' challenge."""
    return (f"Blood{n}Only", f"{n} Blood Only", f"Only cards costing {n} Blood are offered to you.", {
        "fr": (f"{n} sang uniquement", f"On ne vous propose que des cartes coûtant {n} sang."),
        "it": (f"Solo {n} sangue", f"Ti vengono offerte solo carte che costano {n} sangue."),
        "de": (f"Nur {n} Blut", f"Dir werden nur Karten angeboten, die {n} Blut kosten."),
        "es": (f"Solo {n} de sangre", f"Solo se te ofrecen cartas que cuestan {n} de sangre."),
        "pt": (f"Só {n} de Sangue", f"Só lhe oferecem cartas que custam {n} de sangue."),
        "tr": (f"Yalnızca {n} Kan", f"Sana yalnızca {n} Kan değerindeki kartlar sunulur."),
        "ru": (f"Только {n} крови", f"Вам предлагают только карты стоимостью {n} крови."),
        "ja": (f"血{n}のみ", f"提示されるのはコストが血{n}のカードだけ。"),
        "ko": (f"피 {n}만", f"피 {n} 비용의 카드만 제시된다."),
        "zhcn": (f"仅限{n}血", f"只会向你提供消耗{n}点鲜血的卡牌。"),
        "zhtw": (f"僅限{n}血", f"只會向你提供消耗{n}點鮮血的卡牌。"),
    })


TEXTS += [blood(n) for n in (1, 2, 3, 4)]

TEXTS += [
    ("BonesOnly", "Bones Only", "Only cards costing Bones are offered to you.", {
        "fr": ("Os uniquement", "On ne vous propose que des cartes coûtant des os."),
        "it": ("Solo ossa", "Ti vengono offerte solo carte che costano ossa."),
        "de": ("Nur Knochen", "Dir werden nur Karten angeboten, die Knochen kosten."),
        "es": ("Solo huesos", "Solo se te ofrecen cartas que cuestan huesos."),
        "pt": ("Só Ossos", "Só lhe oferecem cartas que custam ossos."),
        "tr": ("Yalnızca Kemik", "Sana yalnızca Kemik değerindeki kartlar sunulur."),
        "ru": ("Только кости", "Вам предлагают только карты, стоящие костей."),
        "ja": ("骨のみ", "提示されるのはコストが骨のカードだけ。"),
        "ko": ("뼈만", "뼈 비용의 카드만 제시된다."),
        "zhcn": ("仅限骨头", "只会向你提供消耗骨头的卡牌。"),
        "zhtw": ("僅限骨頭", "只會向你提供消耗骨頭的卡牌。"),
    }),
    ("FreeOnly", "Free Only", "Only free cards are offered to you.", {
        "fr": ("Gratuites uniquement", "On ne vous propose que des cartes gratuites."),
        "it": ("Solo gratuite", "Ti vengono offerte solo carte gratuite."),
        "de": ("Nur kostenlos", "Dir werden nur kostenlose Karten angeboten."),
        "es": ("Solo gratis", "Solo se te ofrecen cartas gratuitas."),
        "pt": ("Só Grátis", "Só lhe oferecem cartas gratuitas."),
        "tr": ("Yalnızca Bedava", "Sana yalnızca bedava kartlar sunulur."),
        "ru": ("Только бесплатные", "Вам предлагают только бесплатные карты."),
        "ja": ("コストなしのみ", "提示されるのはコストのないカードだけ。"),
        "ko": ("무료만", "비용이 없는 카드만 제시된다."),
        "zhcn": ("仅限免费", "只会向你提供无消耗的卡牌。"),
        "zhtw": ("僅限免費", "只會向你提供無消耗的卡牌。"),
    }),
    ("OneSquirrel", "One Squirrel", "After your opening hand, only one Squirrel is left to draw.", {
        "fr": ("Un seul écureuil", "Après votre main de départ, il ne reste qu'un écureuil à piocher."),
        "it": ("Un solo scoiattolo", "Dopo la mano iniziale resta un solo scoiattolo da pescare."),
        "de": ("Ein Eichhörnchen", "Nach deiner Starthand bleibt nur ein Eichhörnchen zum Ziehen übrig."),
        "es": ("Una ardilla", "Tras tu mano inicial, solo queda una ardilla por robar."),
        "pt": ("Um Esquilo", "Depois da mão inicial, só resta um esquilo para comprar."),
        "tr": ("Tek Sincap", "Başlangıç elinden sonra çekilecek tek bir sincap kalır."),
        "ru": ("Одна белка", "После стартовой руки в колоде остаётся только одна белка."),
        "ja": ("リス1匹", "最初の手札の後、引けるリスは1匹しか残らない。"),
        "ko": ("다람쥐 한 마리", "시작 패 이후 뽑을 다람쥐가 한 마리만 남는다."),
        "zhcn": ("一只松鼠", "起手之后，只剩一只松鼠可以抽。"),
        "zhtw": ("一隻松鼠", "起手之後，只剩一隻松鼠可以抽。"),
    }),
]


def skip(key, title, desc, tr):
    """Entry for a 'No X' event challenge; tr maps lang -> (title, description)."""
    return (key, title, desc, tr)


TEXTS += [
    skip("NoBoneAltar", "No Bone Altar", "The bone altar does nothing.", {
        "fr": ("Pas d'autel d'os", "L'autel d'os ne fait rien."), "it": ("Niente altare d'ossa", "L'altare d'ossa non fa nulla."),
        "de": ("Kein Knochenaltar", "Der Knochenaltar bewirkt nichts."), "es": ("Sin altar de huesos", "El altar de huesos no hace nada."),
        "pt": ("Sem Altar de Ossos", "O altar de ossos não faz nada."), "tr": ("Kemik Sunağı Yok", "Kemik sunağı hiçbir şey yapmaz."),
        "ru": ("Без костяного алтаря", "Костяной алтарь ничего не делает."), "ja": ("骨の祭壇なし", "骨の祭壇は何もしない。"),
        "ko": ("뼈 제단 없음", "뼈 제단은 아무것도 하지 않는다."), "zhcn": ("没有骨祭坛", "骨祭坛毫无作用。"), "zhtw": ("沒有骨祭壇", "骨祭壇毫無作用。"),
    }),
    skip("NoMysteriousStones", "No Mysterious Stones", "The mysterious stones do nothing.", {
        "fr": ("Pas de pierres mystérieuses", "Les pierres mystérieuses ne font rien."), "it": ("Niente pietre misteriose", "Le pietre misteriose non fanno nulla."),
        "de": ("Keine mysteriösen Steine", "Die mysteriösen Steine bewirken nichts."), "es": ("Sin piedras misteriosas", "Las piedras misteriosas no hacen nada."),
        "pt": ("Sem Pedras Misteriosas", "As pedras misteriosas não fazem nada."), "tr": ("Gizemli Taşlar Yok", "Gizemli taşlar hiçbir şey yapmaz."),
        "ru": ("Без таинственных камней", "Таинственные камни ничего не делают."), "ja": ("謎の石なし", "謎の石は何もしない。"),
        "ko": ("신비한 돌 없음", "신비한 돌은 아무것도 하지 않는다."), "zhcn": ("没有神秘石头", "神秘石头毫无作用。"), "zhtw": ("沒有神秘石頭", "神秘石頭毫無作用。"),
    }),
    skip("NoMycologists", "No Mycologists", "The Mycologists do nothing.", {
        "fr": ("Pas de mycologues", "Les mycologues ne font rien."), "it": ("Niente micologi", "I micologi non fanno nulla."),
        "de": ("Keine Mykologen", "Die Mykologen bewirken nichts."), "es": ("Sin micólogos", "Los micólogos no hacen nada."),
        "pt": ("Sem Micologistas", "Os micologistas não fazem nada."), "tr": ("Mantarbilimciler Yok", "Mantarbilimciler hiçbir şey yapmaz."),
        "ru": ("Без микологов", "Микологи ничего не делают."), "ja": ("菌類学者なし", "菌類学者は何もしない。"),
        "ko": ("균류학자 없음", "균류학자는 아무것도 하지 않는다."), "zhcn": ("没有真菌学家", "真菌学家毫无作用。"), "zhtw": ("沒有真菌學家", "真菌學家毫無作用。"),
    }),
    skip("NoDeckTrial", "No Deck Trial", "Deck trials do nothing.", {
        "fr": ("Pas d'épreuve de deck", "Les épreuves de deck ne font rien."), "it": ("Niente prove del mazzo", "Le prove del mazzo non fanno nulla."),
        "de": ("Keine Deckprüfung", "Deckprüfungen bewirken nichts."), "es": ("Sin pruebas de mazo", "Las pruebas de mazo no hacen nada."),
        "pt": ("Sem Prova do Baralho", "As provas do baralho não fazem nada."), "tr": ("Deste Sınavı Yok", "Deste sınavları hiçbir şey yapmaz."),
        "ru": ("Без испытаний колоды", "Испытания колоды ничего не делают."), "ja": ("デッキの試練なし", "デッキの試練は何もしない。"),
        "ko": ("덱 시험 없음", "덱 시험은 아무것도 하지 않는다."), "zhcn": ("没有牌组试炼", "牌组试炼毫无作用。"), "zhtw": ("沒有牌組試煉", "牌組試煉毫無作用。"),
    }),
    skip("NoConsumables", "No Consumables", "Item packs give you nothing.", {
        "fr": ("Pas de consommables", "Les sacs d'objets ne vous donnent rien."), "it": ("Niente consumabili", "Gli zaini di oggetti non ti danno nulla."),
        "de": ("Keine Verbrauchsgüter", "Gegenstandsbeutel geben dir nichts."), "es": ("Sin consumibles", "Las mochilas de objetos no te dan nada."),
        "pt": ("Sem Consumíveis", "As mochilas de itens não lhe dão nada."), "tr": ("Sarf Malzemesi Yok", "Eşya çantaları sana hiçbir şey vermez."),
        "ru": ("Без расходников", "Сумки с предметами ничего не дают."), "ja": ("消耗品なし", "アイテムの袋からは何も得られない。"),
        "ko": ("소모품 없음", "아이템 꾸러미는 아무것도 주지 않는다."), "zhcn": ("没有消耗品", "物品背包什么也不给你。"), "zhtw": ("沒有消耗品", "物品背包什麼也不給你。"),
    }),
    skip("NoCampfire", "No Campfire", "Campfires do nothing.", {
        "fr": ("Pas de feu de camp", "Les feux de camp ne font rien."), "it": ("Niente falò", "I falò non fanno nulla."),
        "de": ("Kein Lagerfeuer", "Lagerfeuer bewirken nichts."), "es": ("Sin hogueras", "Las hogueras no hacen nada."),
        "pt": ("Sem Fogueira", "As fogueiras não fazem nada."), "tr": ("Kamp Ateşi Yok", "Kamp ateşleri hiçbir şey yapmaz."),
        "ru": ("Без костра", "Костры ничего не делают."), "ja": ("焚き火なし", "焚き火は何もしない。"),
        "ko": ("모닥불 없음", "모닥불은 아무것도 하지 않는다."), "zhcn": ("没有篝火", "篝火毫无作用。"), "zhtw": ("沒有篝火", "篝火毫無作用。"),
    }),
    skip("NoBuyPelts", "No Buy Pelts", "The Trapper does nothing.", {
        "fr": ("Pas d'achat de fourrures", "Le Trappeur ne fait rien."), "it": ("Niente acquisto di pelli", "Il Cacciatore non fa nulla."),
        "de": ("Kein Fellkauf", "Der Fallensteller bewirkt nichts."), "es": ("Sin comprar pieles", "El Trampero no hace nada."),
        "pt": ("Sem Comprar Peles", "O Caçador não faz nada."), "tr": ("Post Satın Alma Yok", "Tuzakçı hiçbir şey yapmaz."),
        "ru": ("Без покупки шкур", "Охотник ничего не делает."), "ja": ("皮の購入なし", "罠師は何もしない。"),
        "ko": ("가죽 구매 없음", "덫사냥꾼은 아무것도 하지 않는다."), "zhcn": ("没有购买皮草", "猎人毫无作用。"), "zhtw": ("沒有購買皮草", "獵人毫無作用。"),
    }),
    skip("NoTradePelts", "No Trade Pelts", "The Trader does nothing.", {
        "fr": ("Pas d'échange de fourrures", "Le Marchand ne fait rien."), "it": ("Niente scambio di pelli", "Il Mercante non fa nulla."),
        "de": ("Kein Felltausch", "Der Händler bewirkt nichts."), "es": ("Sin trueque de pieles", "El Comerciante no hace nada."),
        "pt": ("Sem Trocar Peles", "O Comerciante não faz nada."), "tr": ("Post Takası Yok", "Tüccar hiçbir şey yapmaz."),
        "ru": ("Без обмена шкур", "Торговец ничего не делает."), "ja": ("皮の取引なし", "商人は何もしない。"),
        "ko": ("가죽 거래 없음", "상인은 아무것도 하지 않는다."), "zhcn": ("没有交易皮草", "商人毫无作用。"), "zhtw": ("沒有交易皮草", "商人毫無作用。"),
    }),
    ("TotemKinship", "Totem Kinship", "The last totem head offered is a tribe from your deck.", {
        "fr": ("Totem de parenté", "La dernière tête de totem proposée est une tribu de votre deck."),
        "it": ("Totem affine", "L'ultima testa di totem offerta è una tribù del tuo mazzo."),
        "de": ("Totem-Verwandtschaft", "Der letzte angebotene Totemkopf ist ein Stamm aus deinem Deck."),
        "es": ("Tótem afín", "La última cabeza de tótem ofrecida es una tribu de tu mazo."),
        "pt": ("Totem Afim", "A última cabeça de totem oferecida é uma tribo do seu baralho."),
        "tr": ("Totem Akrabalığı", "Sunulan son totem başı destendeki bir kabiledir."),
        "ru": ("Родство тотема", "Последняя предложенная голова тотема — племя из вашей колоды."),
        "ja": ("トーテムの絆", "最後に提示されるトーテムの頭は、デッキにある種族になる。"),
        "ko": ("토템 친족", "마지막으로 제시되는 토템 머리는 덱에 있는 부족이다."),
        "zhcn": ("图腾亲缘", "最后提供的图腾头会是你牌组中的族群。"),
        "zhtw": ("圖騰親緣", "最後提供的圖騰頭會是你牌組中的族群。"),
    }),
]


def cs(s):
    """C# string literal."""
    return '"' + s.replace("\\", "\\\\").replace('"', '\\"') + '"'


def generate():
    """Builds the constants and the dictionary entries as C# source."""
    consts, entries = [], []
    for key, title, desc, tr in TEXTS:
        assert set(tr) == set(LANGS), key
        consts.append(f"        /// <summary>{title} challenge title. Also part of the challenge id, so never change it.</summary>\n"
                      f"        public const string {key}Title = {cs(title)};\n\n"
                      f"        /// <summary>{title} challenge description.</summary>\n"
                      f"        public const string {key}Description = {cs(desc)};\n")
        for idx, name in ((0, "Title"), (1, "Description")):
            body = "".join(f"                    [{cs(l)}] = {cs(tr[l][idx])},\n" for l in LANGS)
            entries.append(f"                [{key}{name}] = new Dictionary<string, string>\n                {{\n{body}                }},\n")
    return "\n".join(consts), "".join(entries)


def main():
    src = open(TARGET, encoding="utf-8").read()
    consts, entries = generate()
    src = re.sub(r"( *// <generated-constants>[^\n]*\n).*?( *// </generated-constants>)", lambda m: m.group(1) + consts + m.group(2), src, flags=re.S)
    src = re.sub(r"( *// <generated-entries>[^\n]*\n).*?( *// </generated-entries>)", lambda m: m.group(1) + entries + m.group(2), src, flags=re.S)
    open(TARGET, "w", encoding="utf-8").write(src)
    print(f"wrote {len(TEXTS)} challenges' texts")


if __name__ == "__main__":
    main()
