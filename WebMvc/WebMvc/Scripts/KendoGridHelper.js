var KendoGridHelper = function (options) {
	//instance variable
	var grid, gridRegion, pk, btns,links, checkedIds;
	var currentDataItem;

	var defaults = { gridId: 'grid', gridRegionId: 'gridRegion'};
	var settings = $.extend(defaults, options || {});

	//immediate execute function
	(function constructor() {
		grid = $("#" + settings.gridId).data("kendoGrid");
		gridRegion = $('#' + settings.gridRegionId);
		pk = settings.pk;
		btns = settings.buttons;
		links = settings.actionLinks;
		checkedIds = {};	
	})();

	KendoGridHelper.prototype.dataSource = function () {
		return grid.dataSource;
	};

	KendoGridHelper.prototype.initiate = function () {
		initTopPager();
		cbxRelatedFuns();
		btnEventBinding();
	};

	KendoGridHelper.prototype.searchBtn = function (btnId) {
		var isDateFormatOk = function () {
			var flag = true;
			$('form.query :text.calendar').each(function () {
				var val = this.value;
				if (val !== '' && kendo.parseDate(val, 'yyyy/MM/dd') === null)
				{
					flag = false;
					return false;
				}
			})
			return flag;
		};

		$("#" + btnId).click(function () {
			if (!isDateFormatOk())
			{
				alertify.alert('日期格式錯誤！');
				return;
			}
			//要求資料來源重新讀取(並指定切至第一頁)
			grid.dataSource.page(1);
			//Grid重新顯示資料
			grid.refresh();
			gridRegion.show();
		});
	};

	KendoGridHelper.prototype.markCurrentDataItem = function (obj) {
		var row = $(obj).closest('tr');
		currentDataItem = grid.dataItem(row);
	};

	KendoGridHelper.prototype.setCurrentDataItem = function (item) {
		for (var property in item) {
			currentDataItem.set(property, item[property]);
		}
	};

	var initTopPager = function () {
		if (grid.pager == null || grid.topPager != null)
			return;
		var pageHolder = $('<div class="kendo-topPagerHolder"></div>').prependTo(gridRegion);
		var topPager = $('<div class="k-pager-wrap k-pager-top" />').appendTo(pageHolder);
		grid.topPager = new kendo.ui.Pager(topPager, { dataSource: grid.dataSource, numeric: false, pageable: false });
	};

	var btnEventBinding = function () {
		var holder = $("div.kendo-topPagerHolder");

		(function bindCustomeBtns() {
			var elements = [];
			for (var key in btns) {
				if (typeof btns[key] !== 'function')
					return;

				var btn = $('<input type="button" class="btn btn-default" value="' + key + '">');
				var fn = decorator(btns[key]);
				btn.click(fn);
				elements.push(btn);
			}

			if(elements.length > 0)
			{
				if(holder.length === 0)
				{
					holder = $('<div class="kendo-topPagerHolder"></div>').prependTo(gridRegion);
				}
				holder.append(elements);
			}
		})();

		function decorator(fn) {
			var hasPara = (fn.length > 0);
			if (hasPara)
			{
				var newfn = function () {
					var para = Object.keys(checkedIds);
					fn.call(this, para);
				};
				return newfn;
			}
			return fn;
		};
	};

	var cbxRelatedFuns = function () {
		(function setBtnFns() {
			if(btns != null)
			{
				if (btns["全選"] !== undefined)
				{					
					var fnOn = function () {$(".checkbox", grid.table).each(function () { tagRow.call(this,true); }	); };
					if( typeof btns["全選"] === 'function')
					{
						var oldfn = btns["全選"];
						btns["全選"] = function () {
							fnOn.call(this);
							oldfn.call(this);
						};
					}else
					{
						btns["全選"] = fnOn;
					}
						
				}
				if (btns["取消"] !== undefined)
				{
					var fnOff = function () { $(".checkbox", grid.table).each(function () { tagRow.call(this, false); }); };
					if (typeof btns["取消"] === 'function') {
						var oldfn = btns["取消"];
						btns["取消"] = function () {
							fnOff.call(this);
							oldfn.call(this);
						};
					} else {
						btns["取消"] = fnOff;
					}					
				}
				if (btns["刪除"] !== undefined && typeof btns["刪除"] !== 'function')
				{
					var fn = function () {
						var keys = Object.keys(checkedIds);
						$.ajax({
							url: links.groupDelete,
							data: {
								ids: keys
							},
							type: "POST",
							dataType: "html",
							success: function () {
								grid.dataSource.read({ page: 1, skip: 0 });
								grid.refresh();
								alertify.success("刪除完成!");
							},
							error: function () {
								location.href = "/Error/";
							}
						});
					};

					btns["刪除"] = function () {
						confirm("確認是否刪除?", function (e) {
							if (e)
								fn();
						});
					};
				}
			}
		})();

		(function bindCbxEvent() {
			//on click of the checkbox:
			var fn = function () {
				var checked = this.checked;
				tagRow.call(this, checked);
			};
			grid.table.on("click", ".checkbox", fn);
		})();

		(function rowRestore() {
			var fn = function () {
				var view = grid.dataSource.view();
				for (var i = 0; i < view.length; i++) {
					if (checkedIds[view[i][pk]]) {
						grid.tbody.find("tr[data-uid='" + view[i].uid + "']")
							.addClass("k-state-selected")
							.find(".checkbox")
							.attr("checked", "checked");
					}
				}
			};
			grid.bind("dataBound", fn);
		})();

		var tagRow = function (checked) {
			var row = $(this).closest("tr");
			var dataItem = grid.dataItem(row);
			var key = dataItem[pk];
			this.checked = checked;

			if (checked) {
				//-select the row & add checked checkbox record
				row.addClass("k-state-selected");
				checkedIds[key] = true;
			} else {
				//-remove selection & unchecked checkbox record
				row.removeClass("k-state-selected");
				delete checkedIds[key];
			}
		};
	};
};

KendoGridHelper.extraData = function () {
	var pairs = $("form.query").serializeObject();
	return pairs;
};