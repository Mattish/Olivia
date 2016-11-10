export default function () {
    return <ng.IDirective>{
        template: require<string>('./taketwoPickList.template.html'),
        restrict: "E",
        scope: {
            addedCards: '='
        },
        controller: 'TakeTwoCardPickListController',
        controllerAs: 'vm',
        bindToController: true
    }
}