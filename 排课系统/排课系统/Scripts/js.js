$(function () {
    $.fn.cgPicEffect = function (opts) {
        var o = $.extend(true, {}, {
            big: null,//澶у浘
            small: null,//缂╃暐鍥�
            title: null,//鏍囬
            pre: null,//鍓嶄竴涓�
            next: null,//涓嬩竴涓�
            stop: null,
            start: null,
            btnDisableClass: null,//涓婁竴涓笌涓嬩竴涓棤娉曠偣鍑绘椂鏍峰紡
            current: 0,
            circular: true,// 鏄惁寰幆婊氬姩
            bigEffect: {//澶у浘鍙樻崲鏁堟灉
                type: "slide",//鍙樻崲绫诲瀷鍙彇鍊糵ade/fadein/slide/sameDirectSlide锛屽綋鍙栧€间笉涓鸿繖鍥涗釜涓椂锛岃皟鐢ㄨ嚜瀹氫箟typeFun鍑芥暟
                speed: 300,//鍙樻崲閫熷害
                typeFun: function (obj, o) { },//鍙嚜瀹氫箟鍙樻崲鍑芥暟
                mouseover: function (obj, o) { },//榧犳爣绉诲埌澶у浘涓婅Е鍙戝嚱鏁�
                mouseout: function (obj, o) { }//榧犳爣绉诲紑澶у浘瑙﹀彂鍑芥暟
            },
            smallEffect: {//缂╃暐鍥惧彉鎹㈡晥鏋�
                style: "image",//缂╃暐鍥炬樉绀哄舰寮忥紝鍙彇鍊糹mage/icon
                type: "fade",//缂╃暐鍥惧彉鎹㈡晥鏋滐紝鍙彇鍊糵ade
                opacity: 0.6,
                speed: 200,//鍙樻崲閫熷害
                typeFun: function (obj, o) { },//鑷畾涔夊彉鎹㈠嚱鏁�
                hoverClass: null,//涓哄綋鍓嶅浘鏃跺叿鏈夌殑鏍峰紡
                onMouse: true,//涓簍rue鏃讹紝mouseover瑙﹀彂浜嬩欢锛宖alse锛宑lick瑙﹀彂浜嬩欢
                mouseover: function (obj, o) { },//榧犳爣绉诲埌缂╃暐鍥句笂瑙﹀彂鍑芥暟
                mouseout: function (obj, o) { }//榧犳爣绉诲紑缂╃暐鍥捐Е鍙戝嚱鏁�
            },
            titleEffect: {//鏍囬鍙樻崲鏁堟灉
                type: "fade",//鍙樻崲绫诲瀷锛屽彲鍙栧€间负fade/slide
                speed: 400,//鍙樻崲閫熷害
                cut: 40,//鎴彇瀛楃鏁帮紝鏈皢涓枃瀛楃涓庤嫳鏂囧瓧绗﹁繘琛屽尯鍒�
                typeFun: function (obj, o) { }//鑷畾涔夊彉鎹㈢被鍨�
            },
            auto: 2500,//鑷姩鍙樻崲鏃剁瓑寰呬簨浠讹紝涓簄ull鏃讹紝涓嶈繘琛岃嚜鍔ㄥ彉鎹�
            before: function (obj, o) {
            },//姣忔鍙樻崲涔嬪墠鍥炶皟鍑芥暟
            after: function (obj, o) {
            }//姣忔鍙樻崲缁撴潫鏃剁殑鍥炶皟鍑芥暟
        }, opts);//鎵€鏈夊弬鏁板潎鍦╫瀵硅薄涓�		
        return $(this).each(function () {
            var obj = {};
            var $this = $(this);
            var maxTime = Math.max(o.smallEffect.speed, o.titleEffect.speed, o.bigEffect.speed);
            var changePic = function (index) {
                if (obj.flag) {
                    obj.index = index;
                    if (obj.current != obj.index) {
                        if (typeof o.before == "function") { var result = o.before(obj, o); if (result == false) return; }
                        if (!obj.index && obj.index != 0) {
                            smallEffect(obj.current);
                        } else {
                            smallEffect(obj.index);
                        }
                        disableBtn(obj.current);
                        setTimeout(function () {
                            obj.current = obj.index;
                            if (typeof o.after == "function") { o.after(obj, o); }
                            obj.flag = true;
                        }, maxTime);
                    }
                }
            }
            var bigEffect = function (index) {
                var len = obj.bigLis.length;
                if (o.bigEffect.type == "fade") {
                    obj.flag = false;
                    obj.bigLis.not(obj.bigLis.eq(obj.current)).hide();
                    obj.bigLis.eq(obj.current).stop().fadeOut(o.bigEffect.speed);
                    obj.bigLis.eq(index).stop().fadeIn(o.bigEffect.speed, function () {
                        obj.bigLis.eq(index).stop().show();
                        obj.bigLis.not(obj.bigLis.eq(index)).stop().hide();
                    });
                } else if (o.bigEffect.type == "fadein") {
                    obj.bigLis.hide();
                    obj.bigLis.eq(index).stop().fadeIn(o.bigEffect.speed);
                } else if (o.bigEffect.type == "slide") {
                    var liWidth = obj.bigLis.eq(0).outerWidth(true);
                    var ulWidth = obj.bigLis.eq(0).outerWidth(true) * obj.length;
                    obj.bigUl.stop().animate({ "left": -liWidth * index }, o.bigEffect.speed, function () {
                    })
                } else if (o.bigEffect.type == "sameDirectSlide") {
                    obj.flag = false;
                    var liWidth = obj.bigLis.eq(0).outerWidth(true);
                    var ulWidth = obj.bigLis.eq(0).outerWidth(true) * obj.length, width;
                    if (index > obj.current || index == 0 && obj.current == obj.length - 1) {
                        width = liWidth * 2;
                    } else if (index <= obj.current || index == obj.length - 1 && obj.current == 0) {
                        width = 0;
                    }
                    obj.bigLis.eq(index).css({ "left": width }).show();
                    obj.bigUl.animate({ "left": -width }, o.bigEffect.speed, function () {
                        obj.bigLis.not(obj.bigLis.eq(index)).hide();
                        obj.bigLis.css({ "left": liWidth });
                        obj.bigUl.css({ "left": -liWidth });
                    })
                } else {
                    o.bigEffect.typeFun(obj, o);
                }
            }
            var smallEffect = function (index) {
                if (o.smallEffect.hoverClass) {
                    obj.smallLis.removeClass(o.smallEffect.hoverClass);
                    obj.smallLis.eq(index).addClass(o.smallEffect.hoverClass);
                }
                if (o.smallEffect.type == "fade") {
                    obj.smallLis.css("opacity", o.smallEffect.opacity);
                    obj.smallLis.eq(index).stop().animate({ "opacity": 1 }, o.smallEffect.speed, function () {
                        //纭繚鍙湁褰撳墠缂╃暐鍥惧叿鏈�
                        obj.smallLis.not(obj.smallLis.eq(index)).css({ "opacity": o.smallEffect.opacity });
                        obj.smallLis.eq(index).css({ "opacity": 1 });
                    });
                } else {
                    o.smallEffect.typeFun(obj, o);
                }
                bigEffect(index);
                titleEffect(index);
            }
            var titleEffect = function (index) {
                if (o.titleEffect.type == "fade") {
                    obj.title.children("a").html(obj.bigLinks.eq(index).attr("title").substring(0, o.titleEffect.cut)).attr("href", obj.bigLinks.eq(index).attr("href")).attr("title", obj.bigLinks.eq(index).attr("title"));
                    obj.title.children("a").hide().fadeIn(o.titleEffect.speed, function () {
                        obj.title.children("a").show();
                    });
                } else if (o.titleEffect.type == "slide") {
                    obj.title.children("a").html(obj.bigLinks.eq(index).attr("title").substring(0, o.titleEffect.cut)).attr("href", obj.bigLinks.eq(index).attr("href")).attr("title", obj.bigLinks.eq(index).attr("title"));
                    var titleHeight = obj.title.outerHeight(true);
                    if (obj.title.css("top") == "0px") {
                        obj.title.css("top", -titleHeight).stop().animate({ "top": 0 }, o.titleEffect.speed, function () {
                            obj.title.css("top", 0);
                        });
                    } else if (obj.title.css("bottom") == "0px") {
                        obj.title.css("bottom", -titleHeight).stop().animate({ "bottom": 0 }, o.titleEffect.speed);
                        obj.title.css("bottom", 0);
                    }
                } else {
                    o.titleEffect.typeFun(obj, o);
                }
            }
            var disableBtn = function (current) {
                if (!o.circular) {
                    if (current > 0 && current < obj.length - 1) {
                        obj.next.removeClass(o.btnDisableClass);
                        obj.pre.removeClass(o.btnDisableClass);
                    } else if (current == 0) {
                        obj.pre.addClass(o.btnDisableClass);
                        obj.next.removeClass(o.btnDisableClass);
                    } else if (current == obj.length - 1) {
                        obj.pre.removeClass(o.btnDisableClass);
                        obj.next.addClass(o.btnDisableClass);
                    }
                }
            }
            var init = function () {
                //灏嗗父鐢ㄥ璞″鍒跺埌obj瀵硅薄涓紝鍦ㄥ洖璋冨嚱鏁颁腑鍙洿鎺ヤ娇鐢�
                obj.small = $(o.small, $this);//缂╃暐鍥�
                obj.big = $(o.big, $this);//澶у浘
                obj.pre = $(o.pre, $this);//鍓嶄竴涓寜閽�
                obj.next = $(o.next, $this);//鍚庝竴涓寜閽�
                obj.stop = $(o.stop, $this);
                obj.start = $(o.start, $this);
                obj.bigUl = $("ul", obj.big);//澶у浘涓殑ul瀵硅薄
                obj.bigLis = $(o.big + " li", $this);//澶у浘涓殑li瀵硅薄
                obj.bigLinks = $("a img", obj.bigLis).parent("a");//鑾峰彇鍒板ぇ鍥句腑鐨勫浘鐗囬摼鎺�
                obj.title = $(o.title, $this);//鏍囬
                obj.length = obj.bigLis.length;//闀垮害
                obj.current = o.current;//褰撳墠鐒︾偣鍥句笅鏍囷紝obj.index涓轰笅涓€涓皢鏄剧ず鐨勫厓绱犱笅鏍囷紝
                obj.flag = true;//婊氬姩鏍囪瘑
                obj.stopflag = false;
                if (o.smallEffect.style == "image") {
                    var smallUl = $("<ul></ul>").appendTo(obj.small);
                    for (var i = 0; i < obj.length; i++) {
                        $('<li>' + obj.bigLinks.eq(i).html() + '</li>').appendTo(smallUl);
                    }
                } else if (o.smallEffect.style == "icon") {
                    var smallUl = $("<ul></ul>").appendTo(obj.small);
                    for (var i = 0; i < obj.length; i++) {
                        $('<li></li>').appendTo(smallUl);
                    }
                }
                obj.smallUl = $("ul", obj.small);//缂╃暐鍥句腑ul瀵硅薄
                obj.smallLis = $("li", obj.smallUl);//缂╃暐鍥句腑li瀵硅薄
                if (o.smallEffect.type == "fade") {
                    obj.smallLis.css("opacity", o.smallEffect.opacity);
                    obj.smallLis.eq(obj.current).css("opacity", 1);
                }
                obj.smallLis.eq(obj.current).addClass(o.smallEffect.hoverClass);

                obj.title.append('<a id="" href="" title="" target="_blank"></a>');
                obj.title.children("a").html(obj.bigLinks.eq(obj.current).attr("title").substring(0, o.titleEffect.cut)).attr("href", obj.bigLinks.eq(obj.current).attr("href")).attr("title", obj.bigLinks.eq(obj.current).attr("title"));
                if (o.bigEffect.type == "fade") {
                    obj.big.css("position", "relative");
                    obj.bigLis.css({ "position": "absolute", "top": 0, "left": 0 });
                    obj.bigLis.each(function (index) {
                        $(this).css({ "z-index": obj.length - index });
                        //$(this).css({"z-index":1});
                    })
                    obj.title.css({ "z-index": obj.length + 1 });
                    obj.pre.css({ "z-index": obj.length + 1 });
                    obj.next.css({ "z-index": obj.length + 1 });
                    obj.smallLis.css({ "z-index": obj.length + 1 });
                } else if (o.bigEffect.type == "slide") {
                    var ulWidth = obj.bigLis.eq(0).outerWidth(true) * obj.length;
                    if (obj.big.css("position") != "absolute" && obj.big.css("position") != "relative") {
                        obj.big.css("position", "relative");
                    }
                    obj.bigUl.css({ "position": "absolute", "width": ulWidth, "left": 0 });
                    obj.bigLis.css({ "float": "left", "overflow": "hidden" });

                } else if (o.bigEffect.type == "sameDirectSlide") {
                    if (obj.big.css("position") != "absolute" && obj.big.css("position") != "relative") {
                        obj.big.css("position", "relative");
                    }
                    var liWidth = obj.bigLis.eq(0).outerWidth(true);
                    var ulWidth = obj.bigLis.eq(0).outerWidth(true) * obj.length;
                    obj.bigLis.css({ "position": "absolute", "left": liWidth, "overflow": "hidden" });
                    obj.bigUl.css({ "position": "absolute", "width": ulWidth, "left": -liWidth });
                    obj.bigLis.not(obj.bigLis.eq(obj.current)).hide();
                }
                //changePic(obj.current);
            }
            init();
            var bindEvent = function () {
                //缂╃暐鍥句簨浠�
                obj.smallLis.each(function (index) {
                    var cli = $(this), event = "click";
                    if (o.smallEffect.onMouse) {
                        event = "mouseover";
                    }
                    if (typeof o.smallEffect.mouseover == "function") {
                        $(this).mouseover(function () {
                            o.smallEffect.mouseover(obj, o);
                        });
                    }
                    if (typeof o.smallEffect.mouseout == "function") {
                        $(this).mouseout(function () {
                            o.smallEffect.mouseout(obj, o);
                        });
                    }
                    $("a", cli).click(function (e) {
                        //e.stopPropagation();
                        e.preventDefault();
                        //return false;
                    });
                    cli.unbind(event).bind(event, function (e) {
                        changePic(index);

                    });
                });
                //澶у浘浜嬩欢
                obj.bigLis.each(function () {
                    var thisLi = $(this);
                    if (typeof o.bigEffect.mouseover == "function") {
                        thisLi.mouseover(function (e) {
                            o.bigEffect.mouseover(obj, o);
                        });
                    }
                    if (typeof o.bigEffect.mouseout == "function") {
                        thisLi.mouseout(function () {
                            o.bigEffect.mouseout(obj, o);
                        });
                    }
                });
                obj.pre.click(function () {
                    var index = obj.current - 1;
                    if (o.circular) {
                        if (index < 0) {
                            index = obj.length - 1;
                        }
                    } else {
                        if (index < 0) {
                            index = 0;
                        }
                    }
                    changePic(index);
                });
                obj.next.click(function () {
                    var index = obj.current + 1;
                    if (o.circular) {
                        index = index % obj.length;
                    } else {
                        if (index == obj.length) {
                            index = obj.length - 1;
                        }
                    }
                    changePic(index);
                });
                obj.stop.click(function () {
                    obj.stopflag = true;
                });
                obj.start.click(function () {
                    obj.stopflag = false;
                });
            }
            bindEvent();
            var auto = function () {
                if (obj.stopflag) { return; }
                obj.timer = setInterval(function () {
                    var index = (obj.current + 1) % obj.length;
                    changePic(index);
                }, o.auto + maxTime)
            }
            var clearAuto = function () {
                $this.mouseover(function () {
                    clearInterval(obj.timer);
                }).mouseout(function () {
                    auto();
                });
            }
            if (o.auto) { auto(); clearAuto(); }
        })
    }
});



