var playerSelector = ".embedded-player-container";
waitForElm(playerSelector).then((elm) => {
    selectWatchLiveIfPresent();

    removeNonPlayerElements();
    var app = document.getElementById("app");
    removeAllClassesInChilds(app);

    // Remove video player static image
    var staticImage = ".inset-video-item-image.w-100.img-loaded";
    waitForElm(staticImage).then((img) => {
        img.remove();
    });
});


function selectWatchLiveIfPresent() {
    var modal = document.getElementsByClassName("modal-content")[0];
    if (modal) {
        var liveButton = modal.querySelector(".btn-main");
        liveButton.click();
    }
}

function removeNonPlayerElements() {
    document.querySelector('footer').remove();

    var header = document.getElementsByClassName('sticky-header-wrapper');
    removeAllInCollection(header);

    // Remove bottom container
    removeIfPresent("#app > div > main > div:nth-child(2)");

    // Remove info text
    removeIfPresent("#app > div > main > div > div > div > div > div.p-0.container-lg");
}

function removeIfPresent(selector) {
    var el = document.querySelector(selector);

    if (el) {
        el.remove();
    }
}

function removeAllClassesInChilds(element) {
    if (element.childNodes.length == 0) {
        return;
    }
    if (element.classList.contains("embedded-player-container")) {
        return;
    }
    element.className = '';
    element.childNodes.forEach(removeAllClassesInChilds);
}

function removeAllInCollection(coll) {
    for (let item of coll) {
        item.remove();
    }
}

function waitForElm(selector) {
    return new Promise(resolve => {
        if (document.querySelector(selector)) {
            return resolve(document.querySelector(selector));
        }

        const observer = new MutationObserver(mutations => {
            if (document.querySelector(selector)) {
                resolve(document.querySelector(selector));
                observer.disconnect();
            }
        });

        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    });
}