import Ember from 'ember';

export function compare(params) {
  if(params[3]){  //handle case insensitive conditions if 4 param is passed.
    params[0]= params[0].toLowerCase();
    params[2]= params[2].toLowerCase();
  }
  let v1 = params[0];
  let operator = params[1];
  let v2 = params[2];
  switch (operator) {
    case '==':
      return (v1 == v2);
    case '!=':
      return (Number(v1) != Number(v2));
    case '===':
      return (Number(v1) === Number(v2) || Number(v1)===-1);
    case '<':
      return (v1 < v2);
    case '<=':
      return (v1 <= v2);
    case '>':
      return (v1 > v2);
    case '>=':
      return (v1 >= v2);
    case '&&':
      return !!(v1 && v2);
    case '||':
      return !!(v1 || v2);
    default:
      return false;
  }
}

export default Ember.Helper.helper(compare);