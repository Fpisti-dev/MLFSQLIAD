function CloseTab() {

    console.log('Close tab');


    setTimeout(function () {
        var win = window.open("about:blank", "_self");
        win.close();
    }, 4000)

}