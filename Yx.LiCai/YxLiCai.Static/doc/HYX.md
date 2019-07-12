代码发布提交后，写清楚 要发布的功能点和路径链接
地址 
*添加银行卡/UserSetting/add_bank
*修改取现密码/UserSetting/fetch_pwd_modify
*设置取现密码/UserSetting/fetch_pwd_setting
*个人设置页面/UserSetting/Index
*身份证绑定/UserSetting/uc_setting_status
*修改登录密码/UserSetting/user_setting_pwd
静态资源路径:
js\User\UserSetting\add_bank.js
js\User\UserSetting\fetch_pwd_modify.js
js\User\UserSetting\fetch_pwd_setting.js
js\User\UserSetting\uc_setting_status.js
js\User\UserSetting\user_setting_pwd.js


---------张世晓2015.6.10
//自有资产
1、bin----4个dll
2、view----userassets文件夹
3、静态资源----JS/USERASSETS.JS
//赎回
1、bin----4个dll
2、view----atone文件夹
3、静态资源----JS/atone.JS
#########################################################################################################################
2015年8月14日 16:59:37
①添加项目表一个字段
pdt_collection，所选产品类型ID，先在235上面添加了字段
②添加新表
CREATE TABLE `yxlc`.`pjt_pdtcategory`(  
  `pjt_id` INT(10) NOT NULL DEFAULT 0 COMMENT '项目id',
  `pdt_c_id` INT(10) NOT NULL DEFAULT 0 COMMENT '产品类型id',
  `status` INT(10) NOT NULL DEFAULT 1 COMMENT '状态0：不可用，1可用',
  PRIMARY KEY (`pjt_id`)
) ENGINE=INNODB CHARSET=utf8 COLLATE=utf8_general_ci








