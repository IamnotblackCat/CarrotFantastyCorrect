# CarrotFantastyCorrect
对开源版的保卫萝卜部分BUG进行了修复

主要修复的BUG：
一、在静音状态下，进入关卡页面返回主菜单会导致音乐重新播放。
  BUG原因及修复：在MainSceneState的StateEnter方法里面一行代码直接调用了GameManager播放背景音乐；在播放前加上一个判断，需要播放的时候   才播放。
二、在失败页面点击选择关卡会重复播放失败音效。
  BUG原因及修复：在游戏失败的时候，不要把游戏设置为暂停，避免走萝卜扣血的逻辑，反复播放失败音效。
三、第一次进入某个关卡读取关卡波次数正常，但是如果此时退出当前关卡选择另外的关卡，怪物波次数会出现错误。
  BUG原因及修复：
  此BUG分为两部分解决。
  1）在UI层面上的显示波次数错误。
  原因：在类TopPage的OnEnable（）比类NormalModel的OnEnterPanel（）要更早调用，所以虽然加了重新验证波次数代码但并没有效果；
  解决办法：在更新面板数据的时候再加一次校对。
  2）实际怪物产生的波次数错误。
  原因：在level的关卡文件中设置的怪物产生波次数都为十次；
  解决办法：在生成关卡数据的时候做一个判断，如果playerManager的总波次数比10小，则取小数。
四、奖励物品总是有蛋。
  BUG原因及修复：代码确实设置了拥有三个蛋以后不再产生蛋，但是没有修改随机数的范围，还是会产生4，这个额时候因为没有设置产生4的页面，于   是页面出现的时候默认是上一次显示的物品，所以还是蛋，但是实际上拥有的蛋数量不会增加；解决办法是，检测到拥有三个蛋以后修改随机数范围。
  
  
  对作品内容的一些修改：
   一、为了测试方便，开启了所有的非隐藏关卡，并且在第一关允许建设所有种类炮台。
   二、开启隐藏关卡的条件是孵化出对应关卡的宠物蛋，所有的宠物蛋都已经放在怪物窝里面，可以直接喂养及孵化，孵化成功就可以开启对应的隐藏关卡。
   三、战斗界面添加了一张宇智波鼬的卡片，点击可以释放技能天照，每关卡仅限使用一次。
   四、游戏是PC端，鼠标操作，尚未制作BOSS关卡。
