$('#printInvoice').click(function () {
    Popup($('.invoice')[0].outerHTML);
    function Popup(data) {
        //Hide everything except the main element

        document.querySelector('nav').classList.add('no-print');
        document.querySelector('footer').classList.add('no-print');
        document.querySelector('.sale-details-footer-paragraph').style.setProperty('margin-top', '25px');
        document.querySelector('.toolbar.hidden-print').classList.add('no-print');


        window.print();

        return true;
    }
});