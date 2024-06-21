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
    spaceBetween: 21,
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

$('.aside-filters .swatch-size').click(function () {
    $(this).toggleClass("swatch-size_active")
})

$('.aside-filters .multi-select__item').click(function () {
    $(this).toggleClass("mult-select__item_selected")
})
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

//filter
function filter() {
    var _colorList = new Array();
    $('.swatch-color_active').each(function () {
        _colorList.push(parseInt($(this).attr("colorId")));
    })
    var _brandList = new Array();
    $('.mult-select__item_selected').each(function () {
        _brandList.push(parseInt($(this).attr("brandId")));
    })
    var _sizeList = new Array();
    $('.swatch-size_active').each(function () {
        _sizeList.push($(this).text());
    })
    $.get("/home/Filter/", { cat: parseInt($(".selected-cat").attr("catId")), colorList: JSON.stringify(_colorList), sizeList: JSON.stringify(_sizeList), brandList: JSON.stringify(_brandList) }, (res) => {
        $(".shop-main .products-grid").empty();
        $(res).each(function () {
            console.log(this)
            $(".shop-main .products-grid").append(`
                          <div class="card border-0 mb-3 mb-md-4 mb-xxl-5 text-start">
    <div class="card-img position-relative">
        <a href="/home/productDescription/${this.productId}">
            <img src="${this.productPhoto}" class="card-img-top  rounded-0" width="100%" height="400" alt="...">
        </a>
        <button class="btn position-absolute border-0 text-uppercase fw-500 addToBasket" pId="${this.ProductId}">
            <span> Add To Cart</span>
            
        </button>
    </div>
    <div class="card-body position-relative p-0 mt-3">
        <p class="item-category text-secondary  mb-1">${this.productCategory.categoryName}</p>
        <h6 class="card-title mb-0 fw-normal">
            <a href="/home/productDescription/${this.ProductId}">${this.ProductName}</a>
            <div class="product-card__price d-flex">
                <span class="money price">$${this.ProductPrice}</span>
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
$(".filter-cat").click(function () {
    $(".filter-cat").removeClass("selected-cat")
    $(this).addClass("selected-cat")
    filter()
})
$('.js-filter-color').click(function () {
    $(this).toggleClass("swatch-color_active")
    filter()
})
$('.js-filter-size').click(function () {
    $(this).toggleClass("swatch-size_active")
    filter()
})
$('.js-filter-brand').click(function () {
    $(this).toggleClass("brand-selected")
    filter()
})
$(".js-search-category").keyup(function () {
    console.log($(this).val())
    $.get("/home/searchCategory/", { brand: $(this).val() }, (res) => {
        $(".js-filter-brand").hide()
        $(res).each(function (index, _brand) {
            console.log(_brand)
            $(".js-filter-brand").each(function () {
                $(".js-filter-brand").css("display", "none !important")
                if ($(this).attr("brandId") == _brand.brandId) {
                    $(this).show()
                }
            })
        })
    })
})
//$('').click(fun)
$('.showAddAddress').click(function () {
    $('.my-account__address').hide()
    $('.addAddressForm').removeClass('d-none')
})
$('.show-edit-password').click(function () {
    $('.edit-password').removeClass('d-none')
    $(this).addClass('d-none')
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
$(".btn-delete").click(function () {
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



//basket
var TotalPrice = parseFloat($(".totalPrc").text().replace('$', ''));
function TotalPrcFunc(total) {
    TotalPrice = total;
    if ($("#flat_rate").prop("checked")) {
        $(".totalPrc").text(`$${(total == 0) ? 0.00.toFixed(2) : (TotalPrice + 49).toFixed(2)}`)
        document.cookie = `TotalPrice=${TotalPrice}; path=/;`;
    }
    else if ($("#local_pickup").prop("checked")) {
        $(".totalPrc").text(`$${(total == 0) ? 0.00.toFixed(2) : (TotalPrice + 8).toFixed(2)}`)
        document.cookie = `TotalPrice=${TotalPrice}; path=/;`;
    }
    else {
        $(".totalPrc").text(`$${(total == 0) ? 0.00.toFixed(2) : TotalPrice.toFixed(2)}`)
        document.cookie = `TotalPrice=${TotalPrice}; path=/;`;
    }
    $(".subtotal").text(`$${(TotalPrice - 19).toFixed(2)}`)
}
$(".qty-control__reduce").click(function () {
    if ($(this).siblings("input").val() > 1) {
        $.get("/basket/QuantityReduce/", { basketId: $(this).closest("tr").attr("basketId") }, (res) => {
            $(this).siblings("input").val(res.productQuantity)
            $(this).closest("tr").find(".shopping-cart-total").text(`$${res.price.toFixed(2)}`)
            TotalPrcFunc(res.totalPrice)
        })
    }
})
$(".qty-control__increase").click(function () {
    if ($(this).siblings("input").val() < 10) {
        $.get("/basket/QuantityIncrease/", { basketId: $(this).closest("tr").attr("basketId") }, (res) => {
            $(this).siblings("input").val(res.productQuantity)
            $(this).closest("tr").find(".shopping-cart-total").text(`$${res.price.toFixed(2)}`)
            TotalPrcFunc(res.totalPrice)
        })
    }
})
//$(".qty-control__number").keyup(function () {
//    console.log($(this).siblings("input").val())
//    if ($(this).val() < 10 ) {
//        $.get("/basket/QuantityIncrease/", { basketId: $(this).parent().attr("basketId") }, (res) => {
//            console.log($(this).siblings("input").val())
//            $(this).siblings().first().val(res)
//        })
//    }
//})
$(".remove-cart-item").click(function () {
    $.get("/basket/RemoveCartItem/", { basketId: $(this).closest("tr").attr("basketId") }, (res) => {
        $(this).closest("tr").hide()
        TotalPrcFunc(res.totalPrice)
    })
})
$('.emptyBasket').click(function () {
    $.get("/basket/EmptyBasket", (res) => {
        $('.cart-table tbody tr').hide()
        TotalPrcFunc(res.totalPrice)
    })
})
$(".form-check-input_fill").change(function () {
    if (this.checked) {
        document.cookie = `shippingType=${this.id}; path=/;`;
        $(".form-check-input_fill").not(this).prop("checked", false);
        if (this.id == "flat_rate") {
            $(".totalPrc").text(`$${(TotalPrice == 0) ? 0.00.toFixed(2) : (TotalPrice + 49).toFixed(2)}`)
            document.cookie = `TotalPrice=${TotalPrice + 49}; path=/;`;

        }
        else if (this.id == "local_pickup") {
            $(".totalPrc").text(`$${(TotalPrice == 0) ? 0.00.toFixed(2) : (TotalPrice + 8).toFixed(2)}`)
            document.cookie = `TotalPrice=${TotalPrice + 8}; path=/;`;

        }
        else {
            $(".totalPrc").text(`$${(TotalPrice == 0) ? 0.00.toFixed(2) : (TotalPrice).toFixed(2)}`)
            document.cookie = `TotalPrice=${TotalPrice}; path=/;`;

        }
    } else {
        $(".totalPrc").text(`$${(TotalPrice == 0) ? 0.00.toFixed(2) : (TotalPrice).toFixed(2)}`)
        document.cookie = 'shippingType=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
    }
});

$(".orderLevel").change(function () {

    $.get("/admin/ConfirmOrders/", { levelId: $(this).find("option:selected").val(), cargoId: $(this).attr("cargoId") }, function () {
    });
});

