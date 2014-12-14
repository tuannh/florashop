var cart = {

    add: function (productId) {
        this.showLoading();
        var data = { '': productId };
        $.post("/api/cart/add", data, this.addCartComplete);
    },

    update: function (productId, qty) {
        this.showLoading();

        var data = { id: productId, quatity: qty };
        $.post("/api/cart/update", data, this.updateCartComplete);
    },

    remove: function (productId) {
        this.showLoading();

        var data = { '': productId };
        $.post("/api/cart/remove", data, this.removeCartComplete);
    },

    clear: function () {
        this.showLoading();

        var data = {};
        $.post("/api/cart/clear", data, this.clearCartComplete);
    },

    addCartComplete: function (data) {
        if (data.error == 0) {

            $('#cart-empty').hide();
            $('#cart-not-empty').hide();
            if (data.count > 0) {
                $('#cart-not-empty').show();
                $('#cart-count-val').text(data.count);
                $('#cart-total-val').text(data.total);
            }

            cart.hideLoading();
        }
        else
            alert(data.message);
    },

    updateCartComplete: function (data) {
        if (data.error == 0) {

            // update topcart
            $('#cart-count-val').text(data.count);
            $('#cart-total-val').text(data.total);
            $('#shopcart-total').text(data.total);

            // update maincart
            var tr = $(data.rowid);
            tr.find('.prod-tot').html(data.sum + ' VND');

            var cartUpdate = tr.find('.cart-update');
            cartUpdate.attr('data-change', 0);
            cartUpdate.attr('data-quatity', cartUpdate.val());

            cart.hideLoading();

            $('.btn-order').show();
            $('.clear-cart').show();
        }
        else
            alert(data.message);
    },

    removeCartComplete: function (data) {
        if (data.error == 0) {

            // update topcart
            $('#cart-count-val').text(data.count);
            $('#cart-total-val').text(data.total);
            $('#shopcart-total').text(data.total);

            // update maincart
            var tr = $(data.rowid);
            tr.slideUp();
            tr.remove();

            $('.btn-order').show();
            $('.clear-cart').show();
            if (data.count == 0) {
                $('.btn-order').hide();
                $('.clear-cart').hide();
            }

            cart.hideLoading();
        }
        else
            alert(data.message);
    },

    clearCartComplete: function (data) {

        cart.hideLoading();

        $('#cart-count-val').text(0);
        $('#cart-total-val').text(0);
        $('#shopcart-total').text(0);

        $('.table-product-info').slideUp().remove();
        $('.btn-order').hide();
        $('.clear-cart').hide();
    },

    showLoading: function () {
        $('#mask-loading').show();
    },

    hideLoading: function () {
        setTimeout(function () {
            $('#mask-loading').hide();
        }, 200);
    }

};

$(document).ready(function () {

    // check top cart link has item
    $('.top-right #cart a').click(function () {
        var cart = $(this).find('#cart-total').text().trim();
        if ('Giỏ hàng (0)' == cart)
            return false;

        return true;
    })

    $('.cart-add').click(function () {
        var id = $(this).attr('data-id');
        cart.add(id);
    })

    $('.cart-remove').click(function () {
        var ok = confirm('Bạn muốn xóa sản phẩm ra khỏi giỏ hàng?');
        if (ok) {
            var id = $(this).attr('data-id');
            cart.remove(id);
        }
    })

    $('.cart-update').blur(function () {

        if ($(this).attr('data-change') == '1') {
            var id = $(this).attr('data-id');
            var qty = $(this).val();
            cart.update(id, qty);
        }

    }).change(function () {
        var qty = parseInt(this.value);

        if (qty == NaN || qty <= 0) {
            this.value = $(this).attr('data-quatity');
            alert('Số lượng phải là 1 số dương');
        }
        else {
            this.value = qty;
            if ($(this).attr('data-quatity') != this.value)
                $(this).attr('data-change', 1);
        }
    })

    $('.cart-clear').click(function () {
        var ok = confirm("Bạn muốn xóa tất cả sản phẩm trong giỏ hàng không?");
        if (ok)
            cart.clear();
    })

    $('.cart-link').click(function () {
        location.href = $(this).attr('data-href');
    })
})