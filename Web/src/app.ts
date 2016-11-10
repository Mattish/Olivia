import Main from './main.controller';
import MainDirective from './main.directive';

require('angular');

console.log("Hello World");

angular.module('Olivia',[])
.controller('MainController',Main)
.directive('main',MainDirective);

console.log("Hello from app");