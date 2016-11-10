export default function (): ng.IDirective {
    return {
        restrict: 'AE',
        require: ['ngModel'],
        template: require<string>('./main.template.html'),
        controller: 'MainController'
    };
}