import MainController from './taketwo.controller';
import MainDirective from './taketwo.directive';
import CardPickListDirective from './cardPickList/taketwoPickList.directive';
import CardListDirective from './cardPickList/taketwoList.directive';
import CardPickListController from './cardPickList/taketwoPickList.controller';
import CardBigDirective from './cardBig/taketwoCardBig.directive';
import CardBigController from './cardBig/taketwoCardBig.controller';
require('angular');
require('angular-filter');
require('./taketwo.css');

angular.module('Olivia-TakeTwo', ['angular.filter'])
    .controller('TakeTwoController', MainController)
    .directive('takeTwo', MainDirective)

    .controller('TakeTwoCardBigController', CardBigController)
    .directive('takeTwoCardBig', CardBigDirective)
    
    .directive('takeTwoCardList', CardListDirective)
    
    .controller('TakeTwoCardPickListController', CardPickListController)
    .directive('takeTwoCardPickList', CardPickListDirective);
