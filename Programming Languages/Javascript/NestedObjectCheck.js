var test = { level1:{ level2:{ level3:'level3'} } };

/**
 * Cleaner way to check a whole chain of variables. 
 * Instead of doing this: if(test && test.level1 && test.level1.level2 && test.level1.level2.level3)
 * You can do this (if(checkNested(test, 'level1', 'level2', 'level3')))
 * This will return true if the path is never undefined.
 * Usage: checkNested(test, 'level1', 'level2', 'level3');
 * @param {any} obj the root object
 * @param {any} level the name of each consecutive level deep in the object
 */
function checkNested(obj, level, ...rest) {
    if (obj === undefined) return false
    if (rest.length == 0 && obj.hasOwnProperty(level)) return true
    return checkNested(obj[level], ...rest)
}

checkNested(test, 'level1', 'level2', 'level3'); // true
checkNested(test, 'level1', 'level2', 'foo'); // false