export default function () {
    return <ng.IDirective>{
        template: require<string>('./taketwoCardBig.template.html'),
        restrict: "E",
        scope: {
            card: '=',
            canPick: '='
        },
        controller: 'TakeTwoCardBigController',
        controllerAs: 'vm',
        bindToController: true
    }
}