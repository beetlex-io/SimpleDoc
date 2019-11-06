Vue.component('page-header', {
    props: ['info', 'page'],
    data: function () {
        return {
            count: 0
        }
    },
    template: '<header class="navbar  bs-docs-nav" id="top" style="margin-bottom:10px;"> \
        <div class= "container" > \
            <div class="navbar-header"> \
                <button class="navbar-toggle collapsed" type="button" data-toggle="collapse" data-target="#bs-navbar" aria-controls="bs-navbar" aria-expanded="false"> \
                    <span class="sr-only">Toggle navigation</span> \
                    <span class="icon-bar"></span> \
                    <span class="icon-bar"></span> \
                    <span class="icon-bar"></span> \
                </button> \
                <a href="/" style="margin-left: -30px;" class="navbar-brand">{{info.title}}</a> \
            </div> \
           <div class="navbar-collapse collapse" role="navigation"> \
            <ul class="nav navbar-nav navbar-right hidden-sm"> \
            <li role="presentation" class="dropdown"> \
                     <a class="dropdown-toggle" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false"> \
                       {{info.selectStyle}} <span class="caret" ></span> \
                    </a> \
                    <ul  class="dropdown-menu"> \
                    <li style="display:block;width:400px;"><div style="padding:10px;"> \
                        <a v-for= "item in info.styles" href="javascript:void(0)" @click="$emit(\'stylechange\', item)" style="display:block;float:left;padding:4px;" > {{ item }}</a></div> \
                </li> \
                </ul> \
            </li> \
            <li v-if="!info.admin"><a href="/admin/">管理</a></li> \
            <li v-if="info.admin"><a href="/">主页</a></li> \
            <li v-if="info.admin"><a href="/admin/" :style="{backgroundColor:page==\'index\'?\'#e2e2e2\':\'\'}">文章</a></li> \
            <li v-if="info.admin" > <a href="/admin/cate.html" :style="{backgroundColor:page==\'cate\'?\'#e2e2e2\':\'\'}">分类</a></li> \
            <li v-if="info.admin" > <a href="/admin/setting.html" :style="{backgroundColor:page==\'setting\'?\'#e2e2e2\':\'\'}" >配置</a></li > \
        </ul> \
    </div> \
        </div> \
    </header>',
})

Vue.component('page-footer', {
    props: ['info'],
    data: function () {
        return {
            count: 0
        }
    },
    template: ' <div class="folder-content"> \
        <p> <a href="https://github.com/IKende/SimpleDoc" target="_blank">Simple doc</a> copyright © 2018 <a href="http://www.ikende.com" target="_blank">ikende.com</a> \
            email: henryfan@msn.com <a href="https://github.com/ikende" target="_blank">github.com</a></p > \
        </div>',
})

Vue.component('category-select', {
    props: ['Selecter'],
    template: '<select class="form-control input-sm" v-model="Selecter.Value" @change="$emit(\'change\', $event.target.value)"> \
        <option value="">无分类</option> \
        <option v-for="item in Selecter.Items" :value="item.ID">{{item.Name}}</option> \
</select>'
});
beetlex.errorHandlers[401] = function (d) {
    document.location.href = "/admin/login.html";
};
var webSiteInfo = { title: '', admin: true, styles: [], selectStyle: '' };
var websiteTitle = new beetlexAction("/WebsiteInfo", null);
websiteTitle.requested = function (r) {
    webSiteInfo.title = r.Title;
    webSiteInfo.styles = r.CodeStyles;
    webSiteInfo.selectStyle = r.SelectStyle;
};
websiteTitle.get();
document.title = 'Simple doc copyright © ikende.com';