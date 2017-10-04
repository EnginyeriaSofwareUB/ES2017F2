# ES2017F2

## Estructura de git y convenio de nombres:

### Git:

La estructura consistirá en una rama **master**, de la que cada **sprint** saldrá la rama **develop** de dicho sprint y al final del spriny esa rama se mergeara con master.

En cada sprint se hará un **tag** con master con todo lo que se haya podido implementar y sobre el tag se haran los **releases**.

Durante cada sprint se irán creando ramas para cada **issue**. Y esta rama es la que se mergeara contra develop.

### Convenio de nombres:

Todas las clases empezaran por mayuscula y cada parabra dentro de ellas también:
~~~
 class Dog;
 class FatDog;
~~~

Los nombres de las variables empezarán por minúscula, y cada palabra adicional por mayusculamayuscula:
   
~~~
float radius;
Vector3 distanceToCenter;
~~~
  
Los nombres de los métodos serán igual que las variables, primera letra minuscula y cada parabra adicional mayuscula.
