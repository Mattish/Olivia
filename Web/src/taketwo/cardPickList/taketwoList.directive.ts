export default function () {
    return <ng.IDirective>{
        template: require<string>('./taketwoList.template.html'),
        restrict: "E",
        scope: {
            cards: '='
        }
    }
}