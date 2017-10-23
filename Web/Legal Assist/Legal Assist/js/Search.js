function PageTransfer() {
    localStorage.setItem("searchText", document.getElementById("search").value);
    document.location.href = "Snippet.html";

}
$(function () {
    var availableTags = [
                "I am being evicted", 
                "How to fight for eviction",
                "what to do when you get an eviction notice",
                "can an eviction be reversed",
                "how can i prolong an eviction",
                "how to drag out an eviction",
                "getting evicted nowhere to go",
                "getting evicted for not paying rent",
                "stop eviction process",
                "how an eviction works",
                "how long does it take to evict someone",
                "how an eviction affects you",
                "apartment eviction process",
    ];
    $("#search").autocomplete({
        source: availableTags
    });
});