//import { each } from "jquery";

$(".products-grid .nav .nav-item .nav-link").click(function () {
    $(".products-grid .nav .nav-item .nav-link").removeClass("active")
    $(this).addClass("active")
})

var swiper = new Swiper(".brands-carousel .mySwiper", {
    slidesPerView: 7,
    spaceBetween: 30,
    centeredSlides: true,
    loop: true,
    autoplay: {
        delay: 2500,
        disableOnInteraction: false,
    },
});
var swiper = new Swiper(".hot-deals .mySwiper", {
    slidesPerView: 4,
    spaceBetween: 28,
    centeredSlides: true,
    loop: true,
    autoplay: {
        delay: 1500,
        disableOnInteraction: false,
    },
    pagination: {
        el: ".hot-deals .swiper-pagination",
        clickable: true,
    },
});
var swiper = new Swiper(".shop-main .products-carousel .mySwiper", {
    slidesPerView: 4,
    spaceBetween: 30,
    centeredSlides: true,
    loop: true,
    pagination: {
        el: ".shop-main .products-carousel .swiper-pagination",
        clickable: true,
    },
    navigation: {
        nextEl: ".shop-main .products-carousel .swiper-button-next",
        prevEl: ".shop-main .products-carousel .swiper-button-prev",
    },
});


$('.js-open-aside').click(function () {
    $("body").css("overflow", "hidden")
    $(".page-overlay").removeClass("d-none")
    $(".aside-filters").css("animation-name", "aside_visible")
})
$('.aside-hide-btn').click(function () {
    $("body").css("overflow", "scroll")
    $(".page-overlay").addClass("d-none")
    $(".aside-filters").css("animation-name", "aside_hide")
})

$('.close-aside-div').click(function () {
    $("body").css("overflow", "scroll")
    $(".page-overlay").addClass("d-none")
    $(".aside-filters").css("animation-name", "aside_hide")
})

$('.cols-size').click(function () {
    switch ($(this).html()) {
        case "2":
            $('.shop-main .products-grid').removeClass('row-cols-md-3');
            $('.shop-main .products-grid').removeClass('row-cols-lg-4');
            break;
        case "3":
            $('.shop-main .products-grid').addClass('row-cols-md-3');
            $('.shop-main .products-grid').removeClass('row-cols-lg-4');
            break;
        case "4":
            $('.shop-main .products-grid').addClass('row-cols-md-3');
            $('.shop-main .products-grid').addClass('row-cols-lg-4');
            break;
    }
})

const selected = $('.select-box .selected');
const optionsList = $('.select-box .options-list');
const options = $('.select-box .option');

selected.click(function () {
    optionsList.toggle()
})

options.click(function () {
    selected.html($(this).html());
    optionsList.hide();
})

$(document).on('click', function (event) {
    if (!$(event.target).closest('.select-box').length) {
        optionsList.hide();
    }
});


//FILTER

//CATEGORY FILTER
$(".aside-filters .filter-cat").click(function () {
    $(".aside-filters .filter-cat").removeClass("selected-cat")
    $(".js-remove-category").remove()
    $(this).addClass("selected-cat")
    $('.filter-active-tags').append(
        `<button class="btn rounded-0 py-1 px-3 btn d-inline-flex align-items-center rounded-0 mb-3 me-3 text-uppercase js-remove-category">
                    <svg xmlns="http://www.w3.org/2000/svg" width="23" height="23" fill="currentColor" class="bi bi-x" viewBox="0 0 16 16">
                        <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708" />
                    </svg>
                    <span class="ms-2">${$(this).text()}</span>
                </button>`)
    filter()
})
$('.filter-active-tags').on("click", ".js-remove-category", function () {
    $(".aside-filters .filter-cat").removeClass("selected-cat")
    $(this).remove()
    filter()
})

//COLOR FILTER
$('.aside-filters .js-filter-color').click(function () {
    $(this).toggleClass("swatch-color_active")
    filter()
})
$('.filter-active-tags').on("click", ".js-remove-color", function () {
    let clickedColorId = $(this).attr("colorId");
    $(".aside-filters .swatch-color_active").each(function () {
        if (clickedColorId == $(this).attr("colorId")) {
            $(this).removeClass("swatch-color_active")
        }
    })
    $(this).remove()
    filter()
})

//SIZE FILTER
$('.aside-filters .js-filter-size').click(function () {
    $(this).toggleClass("swatch-size_active")
    filter()
})
$('.filter-active-tags').on("click", ".js-remove-size", function () {
    let clickedSizeId = $(this).attr("sizeId");
    $(".aside-filters .swatch-size_active").each(function (element) {
        if (clickedSizeId == $(this).attr("sizeId")) {
            $(this).removeClass("swatch-size_active")
        }
    })
    $(this).remove()
    filter()
})

//BRAND FILTER
$('.aside-filters .js-filter-brand').click(function () {
    $(this).toggleClass("brand-selected")
    filter()
})
$('.filter-active-tags').on("click", ".js-remove-brand", function () {
    let clickedBrandId = $(this).attr("brandId");
    $(".aside-filters .brand-selected").each(function (element) {
        if (clickedBrandId == $(this).attr("brandId")) {
            $(this).removeClass("brand-selected")
        }
    })
    $(this).remove()
    filter()
})
$(".aside-filters .js-search-brand").keyup(function () {
    $.get("/home/searchBrand/", { brand: $(this).val() }, (res) => {
        $(".aside-filters .js-filter-brand").hide()
        $(res).each(function (index, _brand) {
            $(".aside-filters .js-filter-brand").each(function () {
                $(".aside-filters .js-filter-brand").css("display", "none !important")
                if ($(this).attr("brandId") == _brand.brandId) {
                    $(this).show()
                }
            })
        })
    })
})

//PRICE FILTER

//MIN PRICE
$('#MinPrice').change(function () {
    var maxPrice = $('#MaxPrice').val()
    var minPrice = $(this).val()
    $('.js-remove-min-price').remove()
    if (minPrice >= 0) {
        if (maxPrice != 0 && maxPrice < minPrice) {
            $('.priceError').text('The minimum value cannot be greater than the maximum!')
            $('.priceError').show()
        } else {
            $('.priceError').hide()
            $('.filter-active-tags').append(
                `<button class="btn rounded-0 py-1 px-3 btn d-inline-flex align-items-center rounded-0 mb-3 me-3 text-uppercase js-remove-min-price">
                    <svg xmlns="http://www.w3.org/2000/svg" width="23" height="23" fill="currentColor" class="bi bi-x" viewBox="0 0 16 16">
                        <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708" />
                    </svg>
                    <span class="ms-2">Min Price: $${minPrice}</span>
                </button>`)
            filter()
        }
    } else {
        $('.js-remove-min-price').remove()
        filter()
    }
    $('.filter-active-tags').on("click", ".js-remove-min-price", function () {
        $('#MinPrice').val("")
        $(this).remove()
        filter()
    })
})

//MAX PRICE
$('#MaxPrice').change(function () {
    var maxPrice = $(this).val()
    var minPrice = $("#MinPrice").val()
    $('.js-remove-max-price').remove()
    if (maxPrice > 0) {
        if (minPrice > maxPrice) {
            $('.priceError').text('The minimum value cannot be greater than the maximum!')
            $('.priceError').show()
        } else {
            $('.priceError').hide()
            $('.filter-active-tags').append(
                `<button class="btn rounded-0 py-1 px-3 btn d-inline-flex align-items-center rounded-0 mb-3 me-3 text-uppercase js-remove-max-price">
                    <svg xmlns="http://www.w3.org/2000/svg" width="23" height="23" fill="currentColor" class="bi bi-x" viewBox="0 0 16 16">
                        <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708" />
                    </svg>
                    <span class="ms-2">Max Price: $${maxPrice}</span>
                </button>`)
            filter()
        }
    } else {
        $('.js-remove-max-price').remove()
        filter()
    }
    $('.filter-active-tags').on("click", ".js-remove-max-price", function () {
        $('#MaxPrice').val("")
        $(this).remove()
        filter()
    })
})

function filter() {
    $('.js-remove-color').remove()
    $('.js-remove-brand').remove()
    $('.js-remove-size').remove()
    var _colorList = new Array();
    $('.aside-filters .swatch-color_active').each(function () {
        var id = $(this).attr("colorId")
        var name = $(this).attr("colorName")
        _colorList.push(parseInt(id));
        $('.filter-active-tags').append(
            `<button class="btn rounded-0 py-1 px-3 btn d-inline-flex align-items-center rounded-0 mb-3 me-3 text-uppercase js-remove-color" colorId = "${id}">
                    <svg xmlns="http://www.w3.org/2000/svg" width="23" height="23" fill="currentColor" class="bi bi-x" viewBox="0 0 16 16">
                        <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708" />
                    </svg>
                    <span class="ms-2">${name}</span>
                </button>`)
    })

    var _brandList = new Array();
    $('.aside-filters .brand-selected').each(function () {
        var id = $(this).attr("brandId")
        var name = $(this).attr("brandName")
        _brandList.push(parseInt(id));
        $('.filter-active-tags').append(
            `<button class="btn rounded-0 py-1 px-3 btn d-inline-flex align-items-center rounded-0 mb-3 me-3 text-uppercase js-remove-brand" brandId = "${id}">
                    <svg xmlns="http://www.w3.org/2000/svg" width="23" height="23" fill="currentColor" class="bi bi-x" viewBox="0 0 16 16">
                        <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708" />
                    </svg>
                    <span class="ms-2">${name}</span>
                </button>`)
    })

    var _sizeList = new Array();
    $('.aside-filters .swatch-size_active').each(function () {
        var id = $(this).attr("sizeId")
        var name = $(this).attr("sizeName")
        _sizeList.push(parseInt(id));
        $('.filter-active-tags').append(
            `<button class="btn rounded-0 py-1 px-3 btn d-inline-flex align-items-center rounded-0 mb-3 me-3 text-uppercase js-remove-size" sizeId = "${id}">
                    <svg xmlns="http://www.w3.org/2000/svg" width="23" height="23" fill="currentColor" class="bi bi-x" viewBox="0 0 16 16">
                        <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708" />
                    </svg>
                    <span class="ms-2">${name}</span>
                </button>`)
    })

    //FILTER AJAX
    $.get("/home/Filter/", { cat: parseInt($(".selected-cat").attr("catId")), colorList: JSON.stringify(_colorList), sizeList: JSON.stringify(_sizeList), brandList: JSON.stringify(_brandList), maxPrice: $('#MaxPrice').val(), minPrice: $('#MinPrice').val() }, (res) => {
        $(".shop-main .products-grid").empty();
        $(res).each(function () {
            $(".shop-main .products-grid").append(`
                          <div class="card border-0 mb-3 mb-md-4 mb-xxl-5 text-start">
    <div class="card-img position-relative">
        <a href="/home/productDescription/${this.productId}">
            <img src="/img/productsPhoto/${this.photos[0].photoName}" class="card-img-top  rounded-0" width="100%" height="400" alt="...">
        </a>
        <button class="btn position-absolute border-0 text-uppercase fw-500 addToBasket" pId="${this.productId}">
            <span> Add To Cart</span>
            
        </button>
    </div>
    <div class="card-body position-relative p-0 mt-3">
        <p class="item-category text-secondary  mb-1">${this.productCategory.categoryName}</p>
        <h6 class="card-title mb-0 fw-normal">
            <a href="/home/productDescription/${this.productId}">${this.productName}</a>
            <div class="product-card__price d-flex">
                <span class="money price">$${this.productPrice}</span>
            </div>
            <div class="product-card__review d-flex align-items-center">
                <div class="reviews-group d-flex">
                    ${generateStars(this.productAverageRating)}
                </div>
                <span class="reviews-note text-secondary ms-1 ">8k+ reviews</span>
            </div>
            <button class="position-absolute top-0 end-0 px-2 lh-1 bg-transparent border-0">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                     class="bi bi-heart" viewBox="0 0 16 16">
                    <path class="text-secondary"
                          d="m8 2.748-.717-.737C5.6.281 2.514.878 1.4 3.053c-.523 1.023-.641 2.5.314 4.385.92 1.815 2.834 3.989 6.286 6.357 3.452-2.368 5.365-4.542 6.286-6.357.955-1.886.838-3.362.314-4.385C13.486.878 10.4.28 8.717 2.01zM8 15C-7.333 4.868 3.279-3.04 7.824 1.143q.09.083.176.171a3 3 0 0 1 .176-.17C12.72-3.042 23.333 4.867 8 15" />
                </svg>
            </button>

            <!-- <a class="add-to-card" href="#"></a> -->
    </div>
</div>
            `)
        })
    })
}

//RESET FILTER
$('.js-reset-filter').click(function () {
    $('.swatch-color_active').removeClass('swatch-color_active')
    $('.brand-selected').removeClass('brand-selected')
    $('.swatch-size_active').removeClass('swatch-size_active')
    $('.selected-cat').removeClass('selected-cat')
    $(".js-remove-category").remove()
    $('.js-remove-max-price').remove()
    $('.js-remove-min-price').remove()
    $('.price-range__info input').val("")
    filter()
})

//FILTER END


//GENERATESTARS
function generateStars(rating) {
    let starsHtml = ``;
    for (let i = 1; i <= 5; i++) {
        if (rating >= i) {
            starsHtml += `
                <svg xmlns="http://www.w3.org/2000/svg" width="11" height="11" fill="currentColor" class="bi bi-star-fill me-1" viewBox="0 0 16 16">
                    <path class="text-warning" d="M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z" />
                </svg>
            `;
        } else if (Math.ceil(rating) == i) {
            starsHtml += `
                <svg xmlns="http://www.w3.org/2000/svg" width="11" height="11" fill="currentColor" class="bi bi-star-half me-1" viewBox="0 0 16 16">
                    <path class="text-warning" d="M5.354 5.119 7.538.792A.52.52 0 0 1 8 .5c.183 0 .366.097.465.292l2.184 4.327 4.898.696A.54.54 0 0 1 16 6.32a.55.55 0 0 1-.17.445l-3.523 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256a.5.5 0 0 1-.146.05c-.342.06-.668-.254-.6-.642l.83-4.73L.173 6.765a.55.55 0 0 1-.172-.403.6.6 0 0 1 .085-.302.51.51 0 0 1 .37-.245zM8 12.027a.5.5 0 0 1 .232.056l3.686 1.894-.694-3.957a.56.56 0 0 1 .162-.505l2.907-2.77-4.052-.576a.53.53 0 0 1-.393-.288L8.001 2.223 8 2.226z" />
                </svg>
            `;
        } else {
            starsHtml += `
                <svg xmlns="http://www.w3.org/2000/svg" width="11" height="11" fill="currentColor" class="bi bi-star me-1" viewBox="0 0 16 16">
                    <path class="text-warning" d="M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.56.56 0 0 0-.163-.505L1.71 6.745l4.052-.576a.53.53 0 0 0 .393-.288L8 2.223l1.847 3.658a.53.53 0 0 0 .393.288l4.052.575-2.906 2.77a.56.56 0 0 0-.163.506l.694 3.957-3.686-1.894a.5.5 0 0 0-.461 0z" />
                </svg>
            `;
        }
    }
    return starsHtml;
}


$('.showAddAddress').click(function () {
    $('.my-account__address').hide()
    $('.addAddressForm').removeClass('d-none')
    $('#addressAddBool').val(true);
})
$('.showEditAddress').click(function () {
    $('.my-account__address-item').hide()
    $('.showAddAddress').hide()
    $('.notice').hide()
    $('#addressEditBool').val(true);
    $(this).closest('.my-account__address-item').next().removeClass('d-none')
})
$('.show-edit-password').click(function () {
    $('.edit-password').removeClass('d-none')
    $(this).addClass('d-none')
    $('#passwordBool').val(true);
    $('.edit-password input').removeAttr('disabled');
})

$(".changeUserStat").click(function () {
    $.ajax({
        url: "/admin/ChangeUserStat/" + $(this).parents("tr").attr("uId"),
        method: "GET",
        success: () => {
            $(this).toggleClass("btn-success")
            $(this).toggleClass("btn-primary")
            if ($(this).hasClass("btn-success")) {
                $(this).html("UNBLOCK")
            } else {
                $(this).html("BLOCK")
            }
        },
        error: () => {
            alert("An error occurred while changing user status.");
        }
    })
})
$(".users-table .btn-delete").click(function () {
    $.ajax({
        url: "/admin/DeleteUser/" + $(this).parents("tr").attr("uId"),
        method: "GET",
        success: () => {
            $(this).parents("tr").hide()
        },
        error: () => {
            alert("An error occurred while changing user status.");
        }
    })
})

$(".page-content").on("click", ".confirm-product", function () {
    var item = $(this)
    $.ajax({
        url: "/admin/confirmproduct/" + item.attr("productId"),
        method: "post",
        success: () => {
            item.closest(".card").remove()
        },
        error: () => {
            alert("xeta")
        }
    })
})
$(".page-content").on("click", ".reject-product", function () {
    var item = $(this)
    $.ajax({
        url: "/admin/rejectproduct/" + item.attr("productId"),
        method: "delete",
        success: () => {
            item.closest(".card").remove()
        },
        error: () => {
            alert("xeta")
        }
    })
})

//BASKET

$('.addToBasket').click(function () {
    $.ajax({
        url: "/user/AddToBasket/" + $(this).attr("pId"),
        method: "GET",
        success: () => {
            alert("Product added to basket successfully.");
        },
        error: () => {
            alert("An error occurred while adding to basket.");
        },
    })
})

var TotalPrice = parseFloat($(".totalPrc").text().replace('$', ''));
function TotalPrcFunc(total) {
    TotalPrice = total + 19;
    let selectedValue = $('input[name="ShippingType"]:checked');
    if (selectedValue.val() != null) {
        $(".totalPrc").text("$" + ((total == 0) ? 0.00.toFixed(2) : (TotalPrice + parseInt(selectedValue.attr("shippingCost"))).toFixed(2)))
        $(".totalPrc").siblings().val((total == 0) ? 0.00.toFixed(2) : (TotalPrice + parseInt(selectedValue.attr("shippingCost"))).toFixed(2))

    } else {
        $(".totalPrc").text("$" + ((total == 0) ? 0.00.toFixed(2) : TotalPrice.toFixed(2)))
        $(".totalPrc").siblings().val((total == 0) ? 0.00.toFixed(2) : TotalPrice.toFixed(2))
    }
    $(".subtotal").text("$" + ((TotalPrice - 19).toFixed(2)))
}
function QuantityControl(element, a) {
    $.get("/basket/QuantityControl/", { basketId: $(element).closest("tr").attr("basketId"), type: a }, (res) => {
        $(element).siblings("input").val(res.productQuantity)
        $(element).closest("tr").find(".shopping-cart-total").text("$" + res.price.toFixed(2))
        TotalPrcFunc(res.totalPrice)
    })
}

$(".cart-table .qty-control__reduce").click(function () {
    QuantityControl($(this), "reduce");
})
$(".cart-table .qty-control__increase").click(function () {
    QuantityControl($(this), "increase")
})
$(".product-single__addtocart .qty-control__reduce").click(function () {
    var inputValue = parseInt($(this).siblings("input").val())
    if (inputValue > 1) {
        inputValue--
        $(this).siblings("input").val(inputValue);
        $(this).siblings("label").text(inputValue);

    }
})
$(".product-single__addtocart .qty-control__increase").click(function () {
    var inputValue = parseInt($(this).siblings("input").val())
    if (inputValue < 10) {
        inputValue++
        $(this).siblings("input").val(inputValue)
        $(this).siblings("label").text(inputValue)
    }
})

$(".remove-cart-item").click(function () {
    $.get("/basket/RemoveCartItem/", { basketId: $(this).closest("tr").attr("basketId") }, (res) => {
        $(this).closest("tr").remove()
        TotalPrcFunc(res.totalPrice)
        if (!$('.cart-table tbody tr').is('*')) {
            $('.emptyBasket').hide()
        }
    })
})
$('.emptyBasket').click(function () {
    $(this).hide()
    $.get("/basket/EmptyBasket", (res) => {
        $('.cart-table tbody').empty()
        TotalPrcFunc(0)
    })
})
$(".form-check-input_fill").change(function () {
    $(".totalPrc").text("$" + ((TotalPrice == 0) ? 0.00.toFixed(2) : (TotalPrice + parseInt($(this).attr("shippingCost"))).toFixed(2)))
    $(".totalPrc").siblings().val((TotalPrice == 0) ? 0.00.toFixed(2) : (TotalPrice + parseInt($(this).attr("shippingCost"))).toFixed(2))
});
$(".orderLevel").change(function () {
    $.get("/admin/ConfirmOrders/", { levelId: $(this).find("option:selected").val(), cargoId: $(this).attr("cargoId") }, function () {
    });
});
$(".baglamaSizeLabel").click(function () {
    $(this).siblings(".baglamaSizeDropdown").show()
})

$('.form-check-input').change(function () {
    if ($("#ship_different_address").prop('checked')) {
        $('.addAddressForm').removeClass('d-none')
    } else {
        $('.addAddressForm').addClass('d-none')
    }
})

//BASKET END



//ADD PRODUCT

$(".colorLabel").click(function () {
    $(".selectColor").css("display", "flex")
})
$(".selectColor .swatch-color").click(function () {
    $(".selectColor .swatch-color").removeClass("swatch-color_active");
    $(".selectColor").hide()
    $(this).addClass("swatch-color_active");
    var colorName = $(this).attr("colorName");
    var colorHexCode = $(this).css("color");
    $(".colorInput").val($(this).attr("colorId"))
    $(".colorLabel").html(`<span class="swatch-color mb-0 js-filter-color" style="color: ${colorHexCode} !important"></span> ${colorName}`)
})

let sizeList = [];
$('.sizes .swatch-size').click(function () {
    $(this).toggleClass("swatch-size_active")
    sizeList = [];
    $('.sizes .swatch-size_active').each(function () {
        sizeList.push($(this).attr("sizeId"))
        $("#ProductSize").val(JSON.stringify(sizeList))
    })
})
$(".brendLabel").click(function () {
    $(".brendDropdown").show()
})
$(".categoryLabel").click(function () {
    $(".categoryDropdown").show()
})
$(".dropdown li").click(function () {
    $(this).siblings().removeClass('dropdown-active');
    $(this).closest(".position-relative").find("input").val($(this).attr("id"));
    $(this).closest(".position-relative").find("label").text($(this).text());
    $(this).closest(".dropdown").hide();
    $(this).addClass('dropdown-active');
});

$('.shop-checkout .dropdown-active').each(function () {
    $(this).closest(".position-relative").find("input").val($(this).attr("id"));
    $(this).closest(".position-relative").find("label").text($(this).text());
})
$(document).on('click', function (event) {
    if (!$(event.target).closest('.selectColor').length && !$(event.target).is('.colorLabel')) {
        $(".selectColor").hide();
    }
    if (!$(event.target).closest('.brendDropdown').length && !$(event.target).is('.brendLabel')) {
        $(".brendDropdown").hide();
    }
    if (!$(event.target).closest('.categoryDropdown').length && !$(event.target).is('.categoryLabel')) {
        $(".categoryDropdown").hide();
    }
    if (!$(event.target).closest('.baglamaSizeDropdown').length && !$(event.target).is('.baglamaSizeLabel')) {
        $(".baglamaSizeDropdown").hide();
    }
});
if (document.querySelector("#ProductPhoto") != null) {
    document.querySelector("#ProductPhoto").addEventListener("change", function () {
        let item = document.createElement('div');
        item.className = 'product-img'
        let img = document.createElement('img');
        img.src = URL.createObjectURL(this.files[0]);
        img.width = 100;
        img.height = 150;
        item.appendChild(img);
        item.appendChild(this.cloneNode(true))
        this.value = null
        item.addEventListener('click', function () {
            this.remove()
        })
        document.querySelector('.product-imgs').appendChild(item)
    })
}

//ADD PRODUCT END
$(".product-single__image-item img").attr("src", $(".selected-img").children().attr("src"))
$(".images .left .img").click(function () {
    $(".images .left .img").removeClass("selected-img")
    $(this).addClass("selected-img")
    $(".product-single__image-item img").attr("src", $(this).children().attr("src"))
})

//SEARCH

$('.show-search .bi-search').click(function () {
    $(this).hide()
    $('.show-search .bi-x-lg').show()
    $('.search-popup').show()
})
$('body').on('click', '.show-search .bi-x-lg', function () {
    $(this).hide()
    $('.show-search .bi-search').show()
    $('.search-popup').hide()
})

$(".search-field__input").keyup(function () {
    var _inputValue = $(this).val()
    $('.search-popup__submit').show()
    $('.search-popup__reset').hide()
    if (_inputValue != "") {
        $('.search-popup__reset').show()
        $('.search-popup__submit').hide()
    }
    $(".search-popup__results .search-result").empty()
    if ($(this).val() != "") {
        $.ajax({
            url: "/home/SearchProduct/" + _inputValue,
            method: "GET",
            success: (res) => {
                $(res).each(function () {
                    $(".search-popup__results .search-result").append(`
                          <div class="card border-0 mb-3 mb-md-4 mb-xxl-5 text-start">
    <div class="card-img position-relative">
        <a href="/home/productDescription/${this.productId}">
            <img src="/img/productsPhoto/${this.photos[0].photoName}" class="card-img-top  rounded-0" width="100%" height="400" alt="...">
        </a>
        <button class="btn position-absolute border-0 text-uppercase fw-500 addToBasket" pId="${this.productId}">
            <span> Add To Cart</span>
            
        </button>
    </div>
    <div class="card-body position-relative p-0 mt-3">
        <p class="item-category text-secondary  mb-1">${this.productCategory.categoryName}</p>
        <h6 class="card-title mb-0 fw-normal">
            <a href="/home/productDescription/${this.productId}">${this.productName}</a>
            <div class="product-card__price d-flex">
                <span class="money price">$${this.productPrice}</span>
            </div>
            <div class="product-card__review d-flex align-items-center">
                <div class="reviews-group d-flex">
                    ${generateStars(this.productAverageRating)}
                </div>
                <span class="reviews-note text-secondary ms-1 ">8k+ reviews</span>
            </div>
            <button class="position-absolute top-0 end-0 px-2 lh-1 bg-transparent border-0">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                     class="bi bi-heart" viewBox="0 0 16 16">
                    <path class="text-secondary"
                          d="m8 2.748-.717-.737C5.6.281 2.514.878 1.4 3.053c-.523 1.023-.641 2.5.314 4.385.92 1.815 2.834 3.989 6.286 6.357 3.452-2.368 5.365-4.542 6.286-6.357.955-1.886.838-3.362.314-4.385C13.486.878 10.4.28 8.717 2.01zM8 15C-7.333 4.868 3.279-3.04 7.824 1.143q.09.083.176.171a3 3 0 0 1 .176-.17C12.72-3.042 23.333 4.867 8 15" />
                </svg>
            </button>

            <!-- <a class="add-to-card" href="#"></a> -->
    </div>
</div>
                   `)
                })
            },
            error: () => {
                $(".search-popup__results .search-result").html("<h4>Nothing found</h4>")
            }
        })

    }
})

$('body').on('click', '.search-popup__reset', function () {
    $(this).hide()
    $('.search-popup__submit').show()
    $(".search-field__input").val("")
    $(".search-popup__results .search-result").empty()
})