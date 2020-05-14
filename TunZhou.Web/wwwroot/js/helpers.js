/*
 * JavaScript类型帮助类
 */

class ObjectHelper {
    static isValid(item) {
        if (item === null || item === undefined) {
            return false;
        }
        return true;
    }
}

class StringHelper {
    static isNullOrWhiteSpace(str) {
        if (ObjectHelper.isValid(str)) {
            return str.toString().trim().length <= 0;
        }
        return true;
    }
}

class ArrayHelper {
    static isArrayType(array) {
        if (array instanceof Array === false) {
            throw new TypeError("类型错误");
        }
    }

    static removeEmptyItem(array) {
        ArrayHelper.isArrayType(array);
        for (let i in array) {
            if (array[i].length <= 0) {
                array.splice(i, 1);
            }
        }
        return array;
    }

    static getMaxValue(array) {
        ArrayHelper.isArrayType(array);
        if (array.length <= 0) {
            throw new RangeError("数组没有元素");
        }
        let maxValue = Math.max.apply(null, array)
        return maxValue;
    }
}