grammar Twice;

prog: chunk EOF;

chunk: statement*;

statement: flowStatement SEMI? | singleStatement SEMI;

flowStatement:
    IF expression statement (ELSE statement)? #statementIf
    | WHILE expression statement #statementWhile
    | forLoop #statementFor
    | BLOCK_OPEN chunk BLOCK_CLOSE #statementBlock
    | ASYNC? FUNCTION IDENTIFIER functionType (statement | expression) #statementFunctionDefinition
    ;

functionType: functionArgs COLON type;

functionArgs: PAR_OPEN (IDENTIFIER COLON type (SEPARATOR IDENTIFIER COLON type)*)? PAR_CLOSE;

forLoop:
    FOR BANG? IDENTIFIER IN expression statement;

singleStatement:
    functionCall #functionCallStatement
    | AWAIT expression #awaitStatement
    | EXTERN VARARG? FUNCTION IDENTIFIER COLON type # externFunctionDefinition
    | expression ARROW expression #channelPushStatement
    | (LET | CONST ) IDENTIFIER EQUALS expression #declarationStatement
    | RETURN expression? # returnStatement
    | IDENTIFIER EQUALS expression #assignmentStatement
    ;

functionCall: IDENTIFIER (POINTY_OPEN type POINTY_CLOSE)? PAR_OPEN (expression (SEPARATOR expression)*)? PAR_CLOSE;

expression:
    PAR_OPEN expression PAR_CLOSE #expressionParenthesis
    | BLOCK_OPEN chunk expression BLOCK_CLOSE #expressionBlock
    | BANG expression #expressionNegate
    | REVERSE_ARROW expression #expressionChannelLoad
    | expression DIV expression #expressionDivide
    | expression MULT expression #expressionMultiply
    | expression PLUS expression #expressionAdd
    | expression MINUS expression #expressionSubtract
    | expression MODULO expression #expressionModulo
    | expression UNTIL expression #expressionUntil
    | expression comp expression #expressionComparison
    | expression AND expression #expressionAnd
    | expression OR expression #expressionOr
    | functionCall #expressionFunctionCall
    | AWAIT expression #awaitExpression
    | expression SQUARE_OPEN expression SQUARE_CLOSE #expressionArrayAccess
    | BACKSLASH ASYNC? functionType ARROW (statement | expression) #expressionLambda
    | SQUARE_OPEN (expression (SEPARATOR expression)*)? SQUARE_CLOSE #expressionArrayLiteral
    | (TRUE | FALSE) #expressionBool
    | STRING #expressionString
    | IDENTIFIER #expressionVariable
    | NUMBER #expressionNumber
    ;

type:   type PAR_OPEN (type (SEPARATOR type)*)? PAR_CLOSE #typeFunction
        | GENERIC POINTY_OPEN type POINTY_CLOSE #typeGeneric 
        | TYPE #typeBasic
        ;

comp: COMP | POINTY_OPEN | POINTY_CLOSE;

BLOCK_OPEN: '{';
BLOCK_CLOSE: '}';

PAR_OPEN: '(';
PAR_CLOSE: ')';

SQUARE_OPEN: '[';
SQUARE_CLOSE: ']';

POINTY_OPEN: '<';
POINTY_CLOSE: '>';

SEPARATOR: ',';

SEMI: ';';
COLON: ':';
ARROW: '->';
REVERSE_ARROW: '<-';
BACKSLASH: '\\';

MODULO: '%';
PLUS: '+';
MINUS: '-';
MULT: '*';
DIV: '/';
AND: '&&';
OR: '||';

WHILE: 'while';
IF: 'if';
ELSE: 'else';
FOR: 'for';
IN: 'in';
FUNCTION: 'fn';
LET: 'let';
CONST: 'const';
RETURN: 'return';
ASYNC: 'threaded';
AWAIT: 'await';
EXTERN: 'extern';
VARARG: 'vararg';

DOT: '.';
BANG: '!';
UNTIL: '~';

TRUE: 'true';
FALSE: 'false';

EQUALS: '=';

COMP: (POINTY_OPEN | BANG | POINTY_CLOSE | EQUALS) EQUALS;

TYPE: 'int' | 'bool' | 'double' | 'string' | 'void';
GENERIC: 'array' | 'channel' | 'promise';


IDENTIFIER: [A-Za-z][A-Za-z0-9_]*;
NUMBER: MINUS? (FLOAT | DEC);

FLOAT:  DIGIT+ DOT DIGIT+
        | DOT DIGIT+
        ;

DEC: DIGIT+;

DIGIT: [0-9];

WS: (' ' | '\t' | '\n' | '\r')+ -> channel(HIDDEN);

COMMENT: MINUS MINUS ~('\r' | '\n')* -> channel(HIDDEN);

STRING: '"' (ESC|.)*? '"';
fragment ESC: '\\"' | '\\\\' ;