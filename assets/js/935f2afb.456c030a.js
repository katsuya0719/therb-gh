"use strict";(self.webpackChunkwebsite=self.webpackChunkwebsite||[]).push([[53],{1109:function(e){e.exports=JSON.parse('{"pluginId":"default","version":"current","label":"Next","banner":null,"badge":false,"className":"docs-version-current","isLast":true,"docsSidebars":{"tutorialSidebar":[{"type":"link","label":"\u30c1\u30e5\u30fc\u30c8\u30ea\u30a2\u30eb","href":"/therb-gh/docs/intro","docId":"intro"},{"type":"category","label":"\u4f7f\u3044\u65b9","collapsible":true,"collapsed":true,"items":[{"type":"link","label":"\u30a4\u30f3\u30b9\u30c8\u30fc\u30eb\u65b9\u6cd5","href":"/therb-gh/docs/Usage/HowToInstall","docId":"Usage/HowToInstall"},{"type":"link","label":"THERB\u30e2\u30c7\u30eb\u3092\u4f5c\u6210\u3059\u308b","href":"/therb-gh/docs/Usage/CreateTherbModel","docId":"Usage/CreateTherbModel"},{"type":"link","label":"\u58c1\u4f53\u69cb\u6210\u306e\u767b\u9332","href":"/therb-gh/docs/Usage/RegisterConstruction","docId":"Usage/RegisterConstruction"},{"type":"link","label":"THERB\u30e2\u30c7\u30eb\u3092\u7de8\u96c6\u3059\u308b","href":"/therb-gh/docs/Usage/EditTherbModel","docId":"Usage/EditTherbModel"},{"type":"link","label":"THERB\u30b7\u30df\u30e5\u30ec\u30fc\u30b7\u30e7\u30f3\u3092\u958b\u59cb\u3059\u308b","href":"/therb-gh/docs/Usage/RunSimulation","docId":"Usage/RunSimulation"}]},{"type":"category","label":"Component","collapsible":true,"collapsed":true,"items":[{"type":"link","label":"Modelling","href":"/therb-gh/docs/Component/Modelling","docId":"Component/Modelling"},{"type":"link","label":"IO","href":"/therb-gh/docs/Component/IO","docId":"Component/IO"},{"type":"link","label":"Filter","href":"/therb-gh/docs/Component/Filter","docId":"Component/Filter"},{"type":"link","label":"Simulation","href":"/therb-gh/docs/Component/Simulation","docId":"Component/Simulation"},{"type":"link","label":"Utility","href":"/therb-gh/docs/Component/Utility","docId":"Component/Utility"}]},{"type":"category","label":"Detail","collapsible":true,"collapsed":true,"items":[{"type":"link","label":"THERB interface concept","href":"/therb-gh/docs/Detail/Concept","docId":"Detail/Concept"}]},{"type":"category","label":"NewHasp","collapsible":true,"collapsed":true,"items":[{"type":"link","label":"How to use New Hasp","href":"/therb-gh/docs/NewHasp/HowToUse","docId":"NewHasp/HowToUse"}]},{"type":"category","label":"Reference","collapsible":true,"collapsed":true,"items":[{"type":"link","label":"\u30c7\u30d5\u30a9\u30eb\u30c8\u30d1\u30e9\u30e1\u30fc\u30bf\u306b\u3064\u3044\u3066","href":"/therb-gh/docs/Reference/DefaultParameter","docId":"Reference/DefaultParameter"}]}]},"docs":{"Component/Filter":{"id":"Component/Filter","title":"Filter","description":"\u30c7\u30fc\u30bf\u306e\u30d5\u30a3\u30eb\u30bf\u30ea\u30f3\u30b0\u3092\u884c\u3046\u30b3\u30f3\u30dd\u30fc\u30cd\u30f3\u30c8\u306e\u30ab\u30c6\u30b4\u30ea","sidebar":"tutorialSidebar"},"Component/IO":{"id":"Component/IO","title":"IO","description":"\u30c7\u30fc\u30bf\u306e\u5165\u51fa\u529b\u3092\u884c\u3046\u30b3\u30f3\u30dd\u30fc\u30cd\u30f3\u30c8\u306e\u30ab\u30c6\u30b4\u30ea","sidebar":"tutorialSidebar"},"Component/Modelling":{"id":"Component/Modelling","title":"Modelling","description":"THERB\u30c7\u30fc\u30bf\u306e\u751f\u6210\u3001\u7de8\u96c6\u3092\u884c\u3046\u30b3\u30f3\u30dd\u30fc\u30cd\u30f3\u30c8\u306e\u30ab\u30c6\u30b4\u30ea","sidebar":"tutorialSidebar"},"Component/Simulation":{"id":"Component/Simulation","title":"Simulation","description":"\uff08\u5b88\u7530\u541b\u306b\u30d5\u30a9\u30fc\u30de\u30c3\u30c8\u3092\u304a\u9858\u3044\u3057\u305f\u3044\uff09","sidebar":"tutorialSidebar"},"Component/Utility":{"id":"Component/Utility","title":"Utility","description":"\u30b7\u30df\u30e5\u30ec\u30fc\u30b7\u30e7\u30f3\u306e\u5b9f\u884c\u306b\u95a2\u308f\u308b\u30b3\u30f3\u30dd\u30fc\u30cd\u30f3\u30c8\u306e\u30ab\u30c6\u30b4\u30ea","sidebar":"tutorialSidebar"},"Detail/Concept":{"id":"Detail/Concept","title":"THERB interface concept","description":"THERB-GH\u3067\u306f\u3001Room,Face,Window,Overhang\u3068\u3044\u30464\u3064\u306e\u30aa\u30d6\u30b8\u30a7\u30af\u30c8\u3092\u7528\u3044\u306a\u304c\u3089Rhinoceros\u30c7\u30fc\u30bf\u306e\u30e2\u30c7\u30ea\u30f3\u30b0\u30c7\u30fc\u30bf\u3092THERB\u306e\u5165\u529b\u30c7\u30fc\u30bf\u306b\u5909\u63db\u3057\u3066\u3044\u307e\u3059\u3002","sidebar":"tutorialSidebar"},"intro":{"id":"intro","title":"\u30c1\u30e5\u30fc\u30c8\u30ea\u30a2\u30eb","description":"THERB-GH\u306e\u4f7f\u7528\u65b9\u6cd5\u306e\u6982\u7565\u3092\u8aac\u660e\u3057\u307e\u3059.","sidebar":"tutorialSidebar"},"NewHasp/HowToUse":{"id":"NewHasp/HowToUse","title":"How to use New Hasp","description":"THERB-GH\u3067\u306fNew Hasp\u3068\u3044\u3046\u71b1\u8ca0\u8377\u8a08\u7b97\u30bd\u30d5\u30c8\u3078\u306e\u9023\u643a\u3082\u3067\u304d\u307e\u3059\u3002","sidebar":"tutorialSidebar"},"Reference/DefaultParameter":{"id":"Reference/DefaultParameter","title":"\u30c7\u30d5\u30a9\u30eb\u30c8\u30d1\u30e9\u30e1\u30fc\u30bf\u306b\u3064\u3044\u3066","description":"\u58c1\u4f53\u69cb\u6210","sidebar":"tutorialSidebar"},"Usage/CreateTherbModel":{"id":"Usage/CreateTherbModel","title":"THERB\u30e2\u30c7\u30eb\u3092\u4f5c\u6210\u3059\u308b","description":"Rhino\u306b\u30b8\u30aa\u30e1\u30c8\u30ea\u3092\u30e2\u30c7\u30ea\u30f3\u30b0\u3057\u3001THERB\u30e2\u30c7\u30eb\u3078\u5909\u63db\u3059\u308b\u624b\u9806\u3092\u793a\u3057\u307e\u3059\u3002","sidebar":"tutorialSidebar"},"Usage/EditTherbModel":{"id":"Usage/EditTherbModel","title":"THERB\u30e2\u30c7\u30eb\u3092\u7de8\u96c6\u3059\u308b","description":"THERB\u30e2\u30c7\u30eb\u3092\u7de8\u96c6\u3059\u308b\uff08\u4f8b. \u5916\u58c1\u3092\u5909\u66f4\u3059\u308b\u306a\u3069\uff09\u624b\u9806\u3092\u793a\u3057\u307e\u3059\u3002","sidebar":"tutorialSidebar"},"Usage/HowToInstall":{"id":"Usage/HowToInstall","title":"\u30a4\u30f3\u30b9\u30c8\u30fc\u30eb\u65b9\u6cd5","description":"\u30a4\u30f3\u30b9\u30c8\u30fc\u30eb\u65b9\u6cd5","sidebar":"tutorialSidebar"},"Usage/RegisterConstruction":{"id":"Usage/RegisterConstruction","title":"\u58c1\u4f53\u69cb\u6210\u306e\u767b\u9332","description":"web\u30a2\u30d7\u30ea\u3092\u4f7f\u7528\u3057\u3066\u58c1\u4f53\u69cb\u6210\u3092\u767b\u9332\u3057\u3001\u305d\u306e\u30c7\u30fc\u30bf\u3092\u4f7f\u7528\u3057\u3066\u3001\u58c1\u4f53\u69cb\u6210\u3092\u7de8\u96c6\u3059\u308b\u3053\u3068\u304c\u3067\u304d\u307e\u3059","sidebar":"tutorialSidebar"},"Usage/RunSimulation":{"id":"Usage/RunSimulation","title":"THERB\u30b7\u30df\u30e5\u30ec\u30fc\u30b7\u30e7\u30f3\u3092\u958b\u59cb\u3059\u308b","description":"THERB\u306e\u30e2\u30c7\u30eb\u304c\u69cb\u7bc9\u3067\u304d\u305f\u3089\u3001RunSimulation\u30b3\u30f3\u30dd\u30fc\u30cd\u30f3\u30c8\u3092\u4f7f\u7528\u3057\u3066\u8a08\u7b97\u3092\u5b9f\u884c\u3067\u304d\u307e\u3059\u3002","sidebar":"tutorialSidebar"}}}')}}]);